   M S B A M L   `   `   `  ����B  >Bibliya, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:BibliyaBibliya  5   	       Bibliya.BookReader   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Bibliya     5   	   -   5          � Title$  
BookReader��5   	   $	��450��6   $	��800��6)    � WindowStartupLocation$ CenterScreen=�6L   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ Search��5      $��0��6"   $
��Left=�6/   $��10q�6J   $	��300��6V   
���   �6�   �� 5   
   -   5        $��0��5      $��Right=�6   $��10q�6:   .��6]   фильтры6f   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ bookGrid��5      $��1��6%   $��10q�62      AutoGenerateColumns .    CanUserAddRows .     SelectionMode$ Single=�6p   6�   �� 5   
   -   5        $��
10,0,10,10q�5      $��
Назад��6%   $��2��65   $
��Left=�6B   6]   5      5      