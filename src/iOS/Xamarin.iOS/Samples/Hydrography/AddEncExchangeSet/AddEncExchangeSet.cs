﻿// Copyright 2018 Esri.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific
// language governing permissions and limitations under the License.

using System;
using System.Collections.Generic;
using ArcGISRuntime.Samples.Managers;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Hydrography;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI.Controls;
using Foundation;
using UIKit;

namespace ArcGISRuntimeXamarin.Samples.AddEncExchangeSet
{
    [Register("AddEncExchangeSet")]
    [ArcGISRuntime.Samples.Shared.Attributes.Sample(
        "Add ENC Exchange Set",
        "Hydrography",
        "This sample demonstrates how to load an ENC Exchange Set, including its component cells.",
        "This sample automatically downloads ENC data from ArcGIS Online before displaying the map.")]
    [ArcGISRuntime.Samples.Shared.Attributes.OfflineData("9d2987a825c646468b3ce7512fb76e2d")]
    public class AddEncExchangeSet : UIViewController
    {
        // Create and hold a reference to the MapView.
        private readonly MapView _myMapView = new MapView();

        public AddEncExchangeSet()
        {
            Title = "Add an ENC exchange set";
        }

        private async void Initialize()
        {
            // Initialize the map with an oceans basemap.
            _myMapView.Map = new Map(Basemap.CreateOceans());

            // Get the path to the ENC Exchange Set.
            string encPath = DataManager.GetDataFolder("9d2987a825c646468b3ce7512fb76e2d", "ExchangeSetwithoutUpdates", "ENC_ROOT", "CATALOG.031");

            // Create the Exchange Set.
            // Note: this constructor takes an array of paths because so that update sets can be loaded alongside base data.
            EncExchangeSet encExchangeSet = new EncExchangeSet(encPath);

            // Wait for the layer to load.
            await encExchangeSet.LoadAsync();

            // Store a list of data set extent's - will be used to zoom the mapview to the full extent of the Exchange Set.
            List<Envelope> dataSetExtents = new List<Envelope>();

            // Add each data set as a layer.
            foreach (EncDataset encDataSet in encExchangeSet.Datasets)
            {
                EncLayer encLayer = new EncLayer(new EncCell(encDataSet));

                // Add the layer to the map.
                _myMapView.Map.OperationalLayers.Add(encLayer);

                // Wait for the layer to load.
                await encLayer.LoadAsync();

                // Add the extent to the list of extents.
                dataSetExtents.Add(encLayer.FullExtent);
            }

            // Use the geometry engine to compute the full extent of the ENC Exchange Set.
            Envelope fullExtent = GeometryEngine.CombineExtents(dataSetExtents);

            // Set the viewpoint.
            _myMapView.SetViewpoint(new Viewpoint(fullExtent));
        }

        private void CreateLayout()
        {
            // Add MapView to the page.
            View.AddSubviews(_myMapView);
        }

        public override void ViewDidLoad()
        {
            CreateLayout();
            Initialize();

            base.ViewDidLoad();
        }

        public override void ViewDidLayoutSubviews()
        {
            try
            {
                nfloat topMargin = NavigationController.NavigationBar.Frame.Height + UIApplication.SharedApplication.StatusBarFrame.Height;

                // Reposition controls.
                _myMapView.Frame = new CoreGraphics.CGRect(0, 0, View.Bounds.Width, View.Bounds.Height);
                _myMapView.ViewInsets = new UIEdgeInsets(topMargin, 0, 0, 0);

                base.ViewDidLayoutSubviews();
            }
            // Needed to prevent crash when NavigationController is null. This happens sometimes when switching between samples.
            catch (NullReferenceException)
            {
            }
        }
    }
}