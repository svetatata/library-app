   M S B A M L   `   `   `  ����B  >Bibliya, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:BibliyaBibliya  5   	       Bibliya.Filters   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Bibliya     5   	     � Title$  Filters��5   	   $	��450��6   $	��400��6&    � WindowStartupLocation$ CenterScreen=�62   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��2��6!   $��10q�65   $��Фильтры��6A   $
��Bold"�6P   $��16(�6b   $��Center=�6p   �� 5   
   $��1��6   $��10q�6!   $5��/Год публикации (диапазон):��6-   �� 5    
   $��1��6   $��2��6"    �� Orientation$ 
Horizontal=�62   $��Center=�6K   $��10q�6h   .+�5!      �� -   5         ��Name$ YearFromTextBox��5!      $��50��6/   $��5q�6:   $��Center=�6E   $
��2000��6`   �� 5"      $��Center=�6   $��5q�63   -6A   �� 5#      -   5        $ YearToTextBox��5#      $��50��6-   $��5q�68   $��Center=�6C   $
��2024��6^   5$      �� 5'   
   $��3��6   $��10q�6!   $��Авторы:��6-   �� 5(   
   -   5         ��Name$ AuthorsListBox��5(      $��4��6*   $��2��67   $��10q�6K   $	��100��6W    �� SelectionMode$ Multiple=�6d   5*      �� 5,   
   -   5         ��Name$ OnlyAvailableCheckBox��5,   ,   $��6��6   $��10q�6    $$��Только в наличии��6K   �� 5.   
   $��5��6   $��10q�6!   $��Категория:��6-   �� 5/   
   -   5        	 ��Name$	 CategoryComboBox��5/      $��5��6-   $��10q�6:   $��1��6F   $	��200��6V   51      �� 53   
   -   5        $'��!Применить фильтры��53      $��8��6-   $��10q�6:   $��Center=�6F   6c   �� 54   
   -   5        $��1��54      $��Отменить��6!   $��8��64   $��10q�6A   $��Center=�6M   6j   55      56      