//Form1.Designer.cs
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Markdig;

namespace GeminiChatbotApp
{
    public partial class MainForm : Form
    {
        private const string API_KEY = "";
        private const string API_URL = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-pro-latest:generateContent";

        private readonly HttpClient _httpClient;

        public MainForm()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            string userInput = txtUserInput.Text;
            if (string.IsNullOrWhiteSpace(userInput))
                return;

            txtChat.AppendText($"You: {userInput}\r\n");
            txtUserInput.Clear();

            string response = await GetGeminiResponse(userInput);
            txtChat.AppendText($"Gemini: {response}\r\n\r\n");
        }

        private async Task<string> GetGeminiResponse(string userInput)
        {
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = userInput }
                        }
                    }
                }
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, $"{API_URL}?key={API_KEY}");
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Parse the JSON response
            dynamic responseObject = JsonConvert.DeserializeObject(responseContent);
            string generatedText = responseObject.candidates[0].content.parts[0].text;
            return generatedText;
        }

        // Designer-generated InitializeComponent method would go here
        private void InitializeComponent()
        {
            this.txtChat = new System.Windows.Forms.TextBox();
            this.txtUserInput = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();

            // 
            // txtChat
            // 
            this.txtChat.Location = new System.Drawing.Point(12, 12);
            this.txtChat.Multiline = true;
            this.txtChat.Name = "txtChat";
            this.txtChat.ReadOnly = true;
            this.txtChat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChat.Size = new System.Drawing.Size(460, 300);

            // 
            // txtUserInput
            // 
            this.txtUserInput.Location = new System.Drawing.Point(12, 318);
            this.txtUserInput.Name = "txtUserInput";
            this.txtUserInput.Size = new System.Drawing.Size(379, 23);

            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(397, 318);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);

            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtUserInput);
            this.Controls.Add(this.txtChat);
            this.Name = "MainForm";
            this.Text = "Gemini Chatbot";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox txtChat;
        private System.Windows.Forms.TextBox txtUserInput;
        private System.Windows.Forms.Button btnSend;
    }
}