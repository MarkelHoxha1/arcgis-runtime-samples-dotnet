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
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;

namespace ArcGISRuntime.WPF.Viewer
{
    public partial class SourceCode
    {
        private Dictionary<string, string> _sourceFiles;

        public SourceCode()
        {
            InitializeComponent();
        }

        public void LoadSourceCode()
        {
            string path = SampleManager.Current.SelectedSample.Path;

            string csspath = path.Substring(0, path.IndexOf("Samples")) + "SyntaxHighlighting\\highlight.css";
            string jspath = path.Substring(0, path.IndexOf("Samples")) + "SyntaxHighlighting\\highlight.pack.js";

            _sourceFiles = new Dictionary<string, string>();
            FileSelection.Items.Clear();

            string htmlStart =
                "<html>" +
                "<head>" +
                "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">" +
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

            int csIndex = 0;

            foreach (string filepath in Directory.GetFiles(path))
            {
                try
                {
                    string source;

                    if (filepath.EndsWith(".cs"))
                    {
                        csIndex = _sourceFiles.Count;
                        source = File.ReadAllText(filepath);
                        source =
                            htmlStart +
                            "<code class=\"csharp\">" +
                            source +
                            "</code>" +
                            htmlEnd;
                    }
                    else if (filepath.EndsWith(".xaml"))
                    {
                        source = File.ReadAllText(filepath);
                        source = source.Replace("<", "&lt;");
                        source = source.Replace(">", "&gt;");
                        source =
                            htmlStart +
                            "<code class=\"xml\">" +
                            source +
                            "</code>" +
                            htmlEnd;
                    }
                    else
                    {
                        continue;
                    }
                    _sourceFiles[filepath] = source;
                    FileSelection.Items.Add(filepath);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
            }

            FileSelection.SelectedIndex = csIndex;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(FileSelection.Items.Count>0)
            {
                sourceCodeBrowser.NavigateToString(_sourceFiles[FileSelection.SelectedValue.ToString()]);
            }
        }
    }
}