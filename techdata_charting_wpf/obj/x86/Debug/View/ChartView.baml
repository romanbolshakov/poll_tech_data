   M S B A M L   `   `   `  ����  techdata_charting_wpf*    $techdata_charting_wpf.View.ChartView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Y UDynamicDataDisplay, Version=0.3.0.0, Culture=neutral, PublicKeyToken=5b7d744a7263923f=d34http://research.microsoft.com/DynamicDataDisplay/1.0  5      -   5        $	��600��5      .��5	      �   � RowDefinitions  5
   
   �� 5      $��* �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   .+�5   
   �� .@�5      �� $��График��6   .��5   
   �� $��5q�6   
���   �$��1q�60    �� CornerRadius$ 4��6D   ._�5   
   8  2Microsoft.Research.DynamicDataDisplay.ChartPlotter  -   5          Name$ chartPlotter��5      $��5q�60   
�������   HorizontalAxis 5      I  CMicrosoft.Research.DynamicDataDisplay.Charts.HorizontalDateTimeAxis  5      -   5        $��dateAxis��5   ,   
���   �6Q      VerticalAxis 5      K  EMicrosoft.Research.DynamicDataDisplay.Charts.Axes.VerticalIntegerAxis  5      -   5        $��	countAxis��5   )   
���   �6O   3  -Microsoft.Research.DynamicDataDisplay.Plotter   Children. 5      2  ,Microsoft.Research.DynamicDataDisplay.Header  $W��QГрафик значений контроллируемого параметра��6   6P   =  7Microsoft.Research.DynamicDataDisplay.VerticalAxisTitle  5      $)��#Значение параметра��6#   6C   ?  9Microsoft.Research.DynamicDataDisplay.HorizontalAxisTitle  5      $��Дата и время��6%   6?   5  /Microsoft.Research.DynamicDataDisplay.LineGraph  5      -   5          Name$ 	lineGraph��5   '      StrokeThickness$ 2Q�6k   ;	  5Microsoft.Research.DynamicDataDisplay.PointsGraphBase 	  
DataSource 6:   �� 	 �� Path$	 ItemValuesDataSource�=
  7Microsoft.Research.DynamicDataDisplay.ViewportElement2D
 
  Description
 5      :  4Microsoft.Research.DynamicDataDisplay.PenDescription  5    "   5"      S  MMicrosoft.Research.DynamicDataDisplay.Charts.Navigation.CursorCoordinateGraph  5#         YTextMapping& "x=>{return Convert.ToDateTime(x);}63   D  >Microsoft.Research.DynamicDataDisplay.ElementMarkerPointsGraph  5$      -   5          Name$
 temp��5$   6   =  7Microsoft.Research.DynamicDataDisplay.MarkerPointsGraph  5%      5&      5'      5(      �� 5)      $��Данные��6   .��5*      &   System.Windows.Controls.DataGrid     AutoGenerateColumns .     AlternatingRowBackground
 �����   
IsReadOnly . $��5q�5/      ��5+      �� $%	 ItemValuesDataSource.Collection�   Columns 50      0  *System.Windows.Controls.DataGridTextColumn  51      ,  &System.Windows.Controls.DataGridColumn   Header$ Дата\время��6P   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6-   �� $	 	Timestamp�6P     52      $ Значение��6L    6-   �� $	 Value�6L   54      55      56      �� 57   
   $��1��6   $��5q�6   
���   �$��1q�6=   $ 4��6Q   ._�58      �   59      �� 5:      �� 5;      �� 5<      .+�5>      ��  �� Orientation$ 
Horizontal=�6   .+�5?      �� $��0��6   $��5q�6(   $
��Left=�63   $	��200��6N   ��5@      �� $	 ItemCollection� �� SelectedItem 5A      �� $!	 SelectedSingleItemViewModel��� 5B      $��0��6   $	��5,9q�6)   $��Right=�66   �� 5C      .�6!   .�5D      �� $��Описание��6$   5E      5F      5G      � 5H      $��1��6    � ColumnDefinitions 5I      �� 5J      $
��auto �6+   �� 5K      $
��auto �6+   �� 5L      $
��auto �6+   �� 5M      $
��auto �6+   .+�5O      �� $ 
Horizontal=�6!   .+�5P      �� $��С��6$   $	��5,9q�6-   (  "System.Windows.Controls.DatePicker  5Q      $��5q�6%      SelectedDate 60   �� $	 DateFrom� �� Mode$ TwoWay=�5R      �� 5S      $��1��6!   $ 
Horizontal=�61   .+�5T      �� $
��По��6$   $	��5,9q�6.     5U      $��5q�6%    60   �� $	 DateTo�5V      �� 5W      -   5        $��2��5W      $��Обновить��6-   $��5q�6@   ��6K   �� $	 RefreshDataCommand�6x   �� 5X      $��3��6!   $ 
Horizontal=�61   .+�5Y      �� $��<<��6!   $��5q�64    � ToolTip$ Неделя назад��6?   ��6V   �� $	 SwitchToPrevWeekCommand��� 5Z      $��<��6!   $��5q�60   $	��4,0q�6;   $ День назад��6I   ��6^   �� $	 SwitchToPrevDayCommand��� 5[      $��Сегодня��6!   $��5q�63   $	��4,0q�6>   ��6L   �� $	 SwitchToTodayCommand��� 5\      $��>��6!   $��5q�6-   $	��4,0q�68   $ День вперед��6F   ��6\   �� $	 SwitchToNextDayCommand��� 5]      $��>>��6!   $��5q�6.   $ Неделя вперед��69   ��6Q   �� $	 SwitchToNextWeekCommand�5^      5_      �� 5`      $��2��6   $
��Left=�6*   $ 
Horizontal=�6E   .+�5a      �� $8��2Задержка проверки значений��6   $��5q�6B   �� 5b      $��50��6   $��5q�6,   ��67   �� $	 MonitorRequestDelay��� 5c      $
��мс��6   $��5q�6)   �� 5d      $��2��6   $'��!Запустить монитор��6*   $��5q�6F   $��Right=�6Q   ��6m   �� $	 StartValueMonitorCommand��� 5e      $��2��6   $)��#Остановить монитор��6*   $��5q�6G   $��Right=�6R   ��6n   �� $	 StopValueMonitorCommand�5f      �� 5g      $��2��6   $��Right=�6)   $
��5,10q�6E   �� 5h      -   5         ��Name$ lnkHelp��5h        � ToolTip 62   �� $	 HelpText�.�6U   .�5i      �� $��?��6$   5j      5k      5l      5m      5n      5o      