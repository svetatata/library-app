   M S B A M L   `   `   `  ����B  >Bibliya, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:BibliyaBibliya  5   	       Bibliya.RenewBook   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Bibliya     5   	     � Title$  	RenewBook��5   	   $	��450��6   $	��450��6(    � WindowStartupLocation$ CenterScreen=�64   .��5	      � .+�5
   
   �� $
��Left=�6   $	��Top=�6/   $��	20,20,0,0q�6G   $��16(�6Z   $#��Ваш читат. билет��6h   �� 5   
   -   5         ��Name$ 	readerBox��5      $
��Left=�6&   $	��Top=�6A   $��200,20,20,0q�6Y   $	��200��6n    �� DisplayMemberPath$ lastname��6z    �� SelectedValuePath$ id��5       �� 
IsEditable .  �� MaxDropDownHeight$	 150��5      6+   �� 5   
   $
��Left=�6   $	��Top=�6/   $��	20,70,0,0q�6G   $��16(�6Z   $��Ваша книга��6h   �� 5   
   -   5        $ ISBNBox��5      $
��Left=�6$   $	��Top=�6?   $��200,70,20,0q�6W   $	��200��6l   $ title��6x   $ book_id��5       . $	 150��5      �� 5   
   $
��Left=�6   $	��Top=�6/   $��
20,120,0,0q�6G   $��16(�6[   $+��%Новая дата возврата:��6i   (  "System.Windows.Controls.DatePicker  5   
   -   5          Name$ ReturnDatePicker��5      $
��Left=�6/   $	��Top=�6J   $��200,120,20,0q�6b   $	��200��6x   �� 5   
   -   5        $��Продлить��5      $	��100��6$   $��40��60   $��Right=�6<   $��Bottom=�6X   $��	0,0,20,20q�6s   $��16(�6�   6�   �� 5   
   -   5        $��
Назад��5      $	��100��6!   $��40��6-   $
��Left=�69   $��Bottom=�6T   $��20q�6o   $��16(�6{   6�   5      5      