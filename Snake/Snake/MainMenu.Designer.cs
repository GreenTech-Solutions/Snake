using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    partial class MainMenu
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.ContinueGameButton = new Button();
            this.StartNewGameButton = new Button();
            this.SettingsButton = new Button();
            this.AboutButton = new Button();
            this.ExitButton = new Button();
            this.SuspendLayout();
            // 
            // ContinueGameButton
            // 
            this.ContinueGameButton.AutoEllipsis = true;
            this.ContinueGameButton.Location = new Point(133, 12);
            this.ContinueGameButton.Name = "ContinueGameButton";
            this.ContinueGameButton.Size = new Size(162, 56);
            this.ContinueGameButton.TabIndex = 0;
            this.ContinueGameButton.Text = "Continue";
            this.ContinueGameButton.UseVisualStyleBackColor = true;
            // 
            // StartNewGameButton
            // 
            this.StartNewGameButton.Location = new Point(133, 74);
            this.StartNewGameButton.Name = "StartNewGameButton";
            this.StartNewGameButton.Size = new Size(162, 56);
            this.StartNewGameButton.TabIndex = 1;
            this.StartNewGameButton.Text = "New Game";
            this.StartNewGameButton.UseVisualStyleBackColor = true;
            this.StartNewGameButton.Click += new EventHandler(this.StartNewGameButton_Click);
            // 
            // SettingsButton
            // 
            this.SettingsButton.Location = new Point(133, 136);
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.Size = new Size(162, 56);
            this.SettingsButton.TabIndex = 2;
            this.SettingsButton.Text = "Settings";
            this.SettingsButton.UseVisualStyleBackColor = true;
            // 
            // AboutButton
            // 
            this.AboutButton.Location = new Point(133, 197);
            this.AboutButton.Name = "AboutButton";
            this.AboutButton.Size = new Size(162, 56);
            this.AboutButton.TabIndex = 3;
            this.AboutButton.Text = "About";
            this.AboutButton.UseVisualStyleBackColor = true;
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new Point(133, 259);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new Size(162, 56);
            this.ExitButton.TabIndex = 4;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            // 
            // MainMenu
            // 
            this.AutoScaleMode = AutoScaleMode.Inherit;
            this.BackgroundImage = Resources.SnakeMain;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.ClientSize = new Size(444, 329);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.AboutButton);
            this.Controls.Add(this.SettingsButton);
            this.Controls.Add(this.StartNewGameButton);
            this.Controls.Add(this.ContinueGameButton);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Name = "MainMenu";
            this.Text = "Snake";
            this.ResumeLayout(false);

        }

        #endregion

        private Button ContinueGameButton;
        private Button StartNewGameButton;
        private Button SettingsButton;
        private Button AboutButton;
        private Button ExitButton;
    }
}

