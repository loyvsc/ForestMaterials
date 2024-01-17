using System.Windows;

namespace BuildMaterials.Extensions
{
    public static class MessageBoxExtension
    {
        public static Wpf.Ui.Controls.MessageBoxResult ShowDialogAsync(this Window owner, string text, string title, string primaryButton = "", SymbolRegular primaryIcon= 0,
            string? secondaryButton = "", SymbolRegular secondIcon = 0)
        {
            Wpf.Ui.Controls.MessageBox dialog = new Wpf.Ui.Controls.MessageBox() { Owner = owner, Title = title, Content = text };
            if (primaryButton != string.Empty)
            {
                dialog.PrimaryButtonIcon = primaryIcon;
                dialog.PrimaryButtonText = primaryButton;
                dialog.CloseButtonText = "Отмена";
                
            }
            else
            {
                dialog.CloseButtonText = "ОК";
            }
            if (secondaryButton != string.Empty)
            {
                dialog.SecondaryButtonText = secondaryButton;
                dialog.SecondaryButtonIcon = secondIcon;
            }            
            return dialog.ShowDialogAsync().Result;
        }
    }
}
