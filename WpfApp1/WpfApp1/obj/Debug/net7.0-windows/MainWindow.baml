   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  5   	       WpfApp1.MainWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp1     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   
����� �.��5	      � .+�5
   
   ��  �� Orientation$ 
Horizontal=�6   $
��Left=�6.   $	��Top=�6I   .+�5      �� -   5        $��Add Note��5      $��	10,10,0,0q�6D   
����p��6W   �� 5      -   5        $
��Edit��5      $��	10,10,0,0q�6A   
��� ���6T   �� 5      -   5        $��Delete��5      $��	10,10,0,0q�6E   
�������6X   �� 5      -   5        $��Clear��5      $��	10,10,0,0q�6G   
���/���6Z   �� 5      -   5        $
��Exit��5      $��	10,10,0,0q�6=   
��� E��6P   5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ NotesDataGrid��5         AutoGenerateColumns$ False��6H   $
��Left=�6d   $��	10,50,0,0q�6   $	��Top=�6�   $	��350��6�   $	��760��6�   ��6*   �� *Notes+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *ID+  5      $ Title��6%    64   �� *Title+  5      $ Content��6%    66   �� *	Content+  5      $ Category��6%    67   �� *
Category+  5      $ CreationDate��6%    6;   �� *FormattedCreationDate+  5      $ ModificationDate��6%    6?   �� *FormattedModificationDate+5      5      5      