   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5   	   B >Bibliya, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:BibliyaBibliya 5   	      Bibliya.BookAcceptance   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   6sys&clr-namespace:System;assembly=mscorlib     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Bibliya     5   	     � Title$  BookAcceptance��5	   	   $	��450��6    $	��350��6-    � WindowStartupLocation$ CenterScreen=�69   .��5
      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��0��6!   $��Center=�61   $��0,0,10,0q�6L   $��Читатель:��6^   �� 5   
   -   5         ��Name$ ReaderComboBox��5   0   $��0��6   $��1��6    $��Center=�6H    �� DisplayMemberPath$ lastname��5       �� SelectedValuePath$ id��5       �� 
IsEditable .  �� MaxDropDownHeight$	 150��5      61   �� 5   
   $��1��6   $��0��6!   $��Center=�61   $��	0,10,10,0q�6L   $��Книга:��6_   �� 5   
   -   5        $��BookCopyComboBox��5   0   $��1��6   $��1��6    $��Center=�6H   $ book_id��5       $ id��5!       . $	 150��5"      �� 5%   
   $��2��6   $��0��6!   $��Center=�61   $��	0,10,10,0q�6L   $ ��Дата принятия:��6_   (  "System.Windows.Controls.DatePicker  5&   
   -   5        $��DatePickerAcceptDate��5&   2   $��2��6   $��1��6"   $��Center=�6N   
	 �� Now
   SelectedDate#
 Z	 5'   	   �� 5*   
   -   5        $��IssueBookButton��5*   B   $��3��6   $��0��6   $��2��6.   $��Принять книгу��5+      $��Center=�6   $	��Top=�6:   $��10q�5,      $	��150��6   6   �� 5.   
   -   5        $��3��5.      $
��Left=�6   $��Bottom=�69   $��40��6T   $��Главная��6`   6r   5/      51      