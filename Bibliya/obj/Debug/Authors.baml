   M S B A M L   `   `   `  ����B  >Bibliya, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:BibliyaBibliya  5   	       Bibliya.Authors   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Bibliya     5   	   -   5          � Title$  Authors��5   	   $	��450��6   $	��800��6&    � WindowStartupLocation$ CenterScreen=�62   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   o� $��0��6   ��5      �� 5      .R�5      ]� $	��Top=�6    $��Stretch=�68   5      .@�5      m� -   5        $��
Книги��5      6&   m� 5      -   5        $+��%Читательские билеты��5      64   m� 5      -   5        $��Издательства��5      6-   m� 5      -   5        $��Авторы��5      6'   �� 5      -   5        $	��100��5      .��66   Выдать6>   �� 5      -   5        $	��100��5      .��68   Принять6A   m� 5      -   5        $��
Выход��5      $��Right=�6&   6B   5      &   System.Windows.Controls.DataGrid  5   
   -	   5        $��
authorGrid��5         AutoGenerateColumns . $��1��6@      CanUserAddRows .     SelectionMode$ Single=�6d   5      �� 5"   
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��Center=�6;   $��10q�6X   .+�5#      �� -
   5        $	��↺��5#      $��10q�6!   $��18(�6o   ��6B   Z� ��6Y   Z� 6o   �� 5$      -   5        $��Добавить��5$      $��10q�6(   64   �� 5%      $	��ID:��6   $��5,0,0,0q�6"   $��Center=�63   �� 5&      -   5         ��Name$ 	IdTextBox��5&      $��10q�6)   $	��120��65   �� 5'      $��	Fullname:��6   $��5,0,0,0q�6(   $��Center=�69   �� 5(      -   5        $ FullnameBox��5(      $��10q�6+   $	��120��67   �� 5)      -   5        $��Изменить��5)      $��10q�6(   64   �� 5*      -   5        $��Удалить��5*      $��10q�6'   63   �� 5+      -   5        $��Книги автора��5+      $��10q�6,   68   5,      �� 5-   
   $q��kВведите ID в случае изменения автора или просмотра его книг��6   $
��Left=�6V   $��8(�6q   $��
184,44,0,0q�6~   $��2��6�   $	��Top=�6�   50      51      