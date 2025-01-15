namespace Korn.Utils.WinForms;
partial class RelocableForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private global::System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    public void MouseDownRelocate(object sender, MouseEventArgs e)
    {
        if (e.Button.Equals(MouseButtons.Left))
        {
            ((Control)sender).Capture = false;
            var m = Message.Create(Handle, 0xa1, new IntPtr(0x2), IntPtr.Zero);
            WndProc(ref m);
        }
    }

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
    }

    protected void EndInitializeComponents()
    {
        Inititialize(this);

        void Inititialize(Control control)
        {
            if (control is Form or Label or Panel && control is not LinkLabel)
                control.MouseDown += MouseDownRelocate;

            foreach (var child in control.Controls)
                Inititialize(child as Control);
        }
    }

    protected override CreateParams CreateParams
    {
        get
        {
            const int CS_DROPSHADOW = 0x20000;
            var cp = base.CreateParams;
            cp.ClassStyle |= CS_DROPSHADOW;
            return cp;
        }
    }

    #endregion
}