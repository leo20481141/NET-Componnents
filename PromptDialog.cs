using System;
using System.Drawing;
using System.Windows.Forms;

public class PromptDialog : Form
{
  TextBox textBox;
  bool closed;
  public string InputText {get; private set;}

  public PromptDialog(string text, string title = null)
  {
    this.AutoScaleMode = AutoScaleMode.Dpi;
    this.Font = SystemFonts.MessageBoxFont;
    this.FormBorderStyle = FormBorderStyle.FixedDialog;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Padding = new Padding(11);
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = title;

    closed = false;

    Label label = new();
    label.AutoSize = true;
    label.Dock = DockStyle.Top;
    label.Text = text;

    Panel textboxSeparator = new();
    textboxSeparator.Dock = DockStyle.Top;
    textboxSeparator.Height = 5;

    textBox = new TextBox();
    textBox.Dock = DockStyle.Top;

    Panel bottomPanelSeparator = new();
    bottomPanelSeparator.Dock = DockStyle.Top;
    bottomPanelSeparator.Height = 11;

    Panel bottomPanel = new();
    bottomPanel.Dock = DockStyle.Top;
    bottomPanel.Height = 24;

    Size okTextSize = TextRenderer.MeasureText("Aceptar", SystemFonts.MessageBoxFont);
    int okWidth = Math.Max(okTextSize.Width, 75);
    okWidth += okWidth * 30 / 100;

    Size cancelTextSize = TextRenderer.MeasureText("Cancelar", SystemFonts.MessageBoxFont);
    int cancelWidth = Math.Max(cancelTextSize.Width, 75);
    cancelWidth += cancelWidth * 30 / 100;

    Button btnOk = new();
    btnOk.Dock = DockStyle.Right;
    btnOk.Padding = new Padding(0);
    btnOk.Width = okWidth;
    btnOk.Text = "Aceptar";
    btnOk.Click += (s, e) => Accept();

    Panel btnSeparator = new();
    btnSeparator.Dock = DockStyle.Right;
    btnSeparator.Width = 5;

    Button btnCancel = new();
    btnCancel.Dock = DockStyle.Right;
    btnCancel.Padding = new Padding(0);
    btnCancel.Width = cancelWidth;
    btnCancel.Text = "Cancelar";
    btnCancel.Click += (s, e) => Cancel();

    bottomPanel.Controls.Add(btnOk);
    bottomPanel.Controls.Add(btnSeparator);
    bottomPanel.Controls.Add(btnCancel);

    this.Controls.Add(bottomPanel);
    this.Controls.Add(bottomPanelSeparator);
    this.Controls.Add(textBox);
    this.Controls.Add(textboxSeparator);
    this.Controls.Add(label);
    this.AcceptButton = btnOk;
    this.CancelButton = btnCancel;
    this.Shown += (s, e) => textBox.Focus();
  }

  protected override void OnLoad(EventArgs e)
  {
    base.OnLoad(e);
    this.PerformLayout();

    int bottom = 0;
    bottom += this.Padding.Top;
    foreach(Control control in this.Controls)
    {
      if(!control.Visible) continue;
      if(control.Bottom > bottom) bottom = control.Bottom;
    }
    bottom += this.Padding.Bottom;
    this.Resize += (s, e) => this.CenterToScreen();
    this.ClientSize = new Size(400, bottom);
  }

  void Accept()
  {
    if(closed) return;
    closed = true;
    InputText = textBox.Text;
    this.DialogResult = DialogResult.OK;
    this.Close();
  }

  void Cancel()
  {
    if(closed) return;
    closed = true;
    InputText = null;
    this.DialogResult = DialogResult.Cancel;
    this.Close();
  }
}