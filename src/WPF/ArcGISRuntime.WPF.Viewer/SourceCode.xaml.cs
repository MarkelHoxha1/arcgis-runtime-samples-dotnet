// Copyright 2018 Esri.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific
// language governing permissions and limitations under the License.

using ArcGISRuntime.Samples.Managers;
using System;

namespace ArcGISRuntime.WPF.Viewer
{
    public partial class SourceCode
    {
        private string _csSource;
        private string _xmlSource;

        public SourceCode()
        {
            InitializeComponent();
            // Grab the appropriate source code and navigate to it in the WebBrowser.
        }

        public void loadSourceCode()
        {
            // Load C sharp source code.
            string path = SampleManager.Current.SelectedSample.Path;
            string sourceCode = "None";

            string csspath = path.Substring(0, path.IndexOf("Samples")) + "SyntaxHighlighting\\highlight.css";
            string jspath = path.Substring(0, path.IndexOf("Samples")) + "SyntaxHighlighting\\highlight.pack.js";
            string jquerypath = path.Substring(0, path.IndexOf("Samples")) + "SyntaxHighlighting\\jquery.min.js";

            // Load the sample from src
            //path = path.Substring(0, path.IndexOf("output")) + "src\\WPF\\ArcGISRuntime.WPF.Viewer\\" + path.Substring(path.IndexOf("Samples")) + "\\" + SampleManager.Current.SelectedSample.FormalName + ".xaml.cs";

            // Load the sample from output
            

            string htmlStart =
                "<html>" +
                "<head>" +
                "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"> " +
                "<link rel=\"stylesheet\" href=\"" + csspath + "\">" +
                "<script type=\"text/javascript\" src=\"" + jspath + "\"></script>" +
                "<script>hljs.initHighlightingOnLoad();</script>" +
                "</head>" +
                "<body>" +
                "<pre>";

            string htmlEnd = 
                "</pre>" +
                "</body>" +
                "</html>";

            string csPath = path.Substring(0, path.IndexOf("output")) + "output\\WPF\\debug\\" + path.Substring(path.IndexOf("Samples")) + "\\" + SampleManager.Current.SelectedSample.FormalName + ".xaml.cs";

            try
            {
                _csSource = System.IO.File.ReadAllText(csPath);
            }
            catch (System.Exception e)
            {
                _csSource = e.Message + "\nAttempted File Path: " + csPath;
            }

            // Build html around the source code

            _csSource =
                 htmlStart +
                "<code class=\"csharp\">" +
                _csSource +
                "</code>" +
                htmlEnd;

            // xaml time

            string xmlPath = path.Substring(0, path.IndexOf("output")) + "output\\WPF\\debug\\" + path.Substring(path.IndexOf("Samples")) + "\\" + SampleManager.Current.SelectedSample.FormalName + ".xaml";

            try
            {
                _xmlSource = System.IO.File.ReadAllText(xmlPath);
            }
            catch (System.Exception e)
            {
                _xmlSource = e.Message + "\nAttempted File Path: " + xmlPath;
            }

            // Build html around the source code
            _xmlSource = _xmlSource.Replace("<", "&lt;");
            _xmlSource = _xmlSource.Replace(">", "&gt;");
            _xmlSource =
                 htmlStart +
                "<code class=\"csharp\">" +
                _xmlSource +
                "</code>" +
                htmlEnd;

            sourceCodeBrowser.NavigateToString(_csSource);
        }
        private void LoadCsharp(object sender, EventArgs e)
        {
            sourceCodeBrowser.NavigateToString(_csSource);
            CsharpButton.IsEnabled = false;
            XmlButton.IsEnabled = true;
        }
        private void LoadXml(object sender, EventArgs e)
        {
            sourceCodeBrowser.NavigateToString(_xmlSource);
            CsharpButton.IsEnabled = true;
            XmlButton.IsEnabled = false;
        }
    }
}