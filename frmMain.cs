using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OmtePdfViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void cmdShow_Click(object sender, EventArgs e)
        {
            WebClient webClient = new WebClient();
            try
            {
                webBrowser1.DocumentText = "working..";
                //var fn = $@"c:\dummy\a{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf";
                if (!Directory.Exists(Properties.Settings.Default.PdfOutputFolder)) Directory.CreateDirectory(Properties.Settings.Default.PdfOutputFolder);
                var fn = $@"{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf";
                fn = Path.Combine(Properties.Settings.Default.PdfOutputFolder, fn);
                webClient.Headers.Add("X-API-KEY", txtApiKey.Text);
                webClient.Headers.Add("Accepr", "application/json, application/*+json");
                var jsonB = webClient.UploadData(txtUrl.Text, "POST", UTF8Encoding.UTF8.GetBytes(txtBody.Text));
                //*var json = webClient.UploadString(txtUrl.Text, "POST", txtBody.Text);
                var json = UTF8Encoding.UTF8.GetString(jsonB);
                var o = JsonConvert.DeserializeObject<Omte>(json);
                byte[] result = Convert.FromBase64String(o.documents[0].content);
                File.WriteAllBytes(fn, result);
                webBrowser1.Navigate(fn);
            }
            catch (Exception ex)
            {
                webBrowser1.DocumentText = ex.Message;
                if (webClient.IsBusy)
                {

                }
            }
        }

        public string lastFileName { get; set; } = "";

        private void cmdSave_Click(object sender, EventArgs e)
        {
            var i = new PersistenceItem()
            {
                ApiKey = txtApiKey.Text,
                Body = txtBody.Text,
                OmteUrl = txtUrl.Text,
            };
            saveFileDialog1.Filter = "Json|*.json";
            saveFileDialog1.Title = "Save settings";
            saveFileDialog1.FileName = lastFileName;
            var r = saveFileDialog1.ShowDialog();
            if (r == DialogResult.OK)
            {
                var fn = saveFileDialog1.FileName;
                File.WriteAllText(fn, JsonConvert.SerializeObject(i));
                this.Text = $"Omte PDF viewer - {new FileInfo(fn).Name }";
            }
        }

        private void cmdLoad_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Json|*.json";
            openFileDialog1.Title = "Load settings";
            var r = openFileDialog1.ShowDialog();
            if (r == DialogResult.OK)
            {
                var fn = openFileDialog1.FileName;
                var fc = File.ReadAllText(fn);
                var s = JsonConvert.DeserializeObject<PersistenceItem>(fc);
                txtApiKey.Text = s.ApiKey;
                txtBody.Text = s.Body;
                txtUrl.Text = s.OmteUrl;
                this.Text = $"Omte PDF viewer - {new FileInfo(fn).Name }";
                lastFileName = new FileInfo(fn).Name;
            }
        }

        private void cmdPrettify_Click(object sender, EventArgs e)
        {
            try
            {
                string jsonFormatted = JValue.Parse(txtBody.Text).ToString(Formatting.Indented);
                txtBody.Text = jsonFormatted;
            }
            catch (Exception ex)
            {
                webBrowser1.DocumentText = ex.Message;
            }
        }
    }




}
