   M S B A M L   `   `   `  ����G  CQRS_Detector, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:QRS_DetectorQRS_Detector  5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      l hSystem.Windows.Controls.DataVisualization.Toolkit, Version=4.0.0.0, Culture=neutral, PublicKeyToken=null�{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting 5      �rclr-namespace:System.Windows.Controls.DataVisualization;assembly=System.Windows.Controls.DataVisualization.Toolkit)System.Windows.Controls.DataVisualization 5      ��clr-namespace:System.Windows.Controls.DataVisualization.Charting.Primitives;assembly=System.Windows.Controls.DataVisualization.Toolkit=System.Windows.Controls.DataVisualization.Charting.Primitives 5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   
MyDarkGrey  �� Color  MyMediumGrey  MyLightGrey  MyLight  MyAzure  MyPurple  MyWhite �� 
TargetType  MyButton �� Property �� Value �� 
TargetType �� Triggers P� Property P� Value  	MyTextBox 	 MyLabel 
 MyBorder 
 MyGrid  MyDataGridRow)   #System.Windows.Controls.DataGridRow  MyDataGridRowHeader:  4System.Windows.Controls.Primitives.DataGridRowHeader=  7System.Windows.Controls.Primitives.DataGridColumnHeader  MyDataGridColumnHeader ! DataGridHeaderBackgroundBrush ]� 
Conditions	 �� Property
 �� Value&   System.Windows.Controls.DataGrid  
MyDataGrid  myScrollBarStyle  MyScrollViewer � ColumnDefinitions � RowDefinitions �� VerticalOffset � Value �� ScrollableHeight � Maximum �� ViewportHeight �� ViewportSize* �� #ComputedVerticalScrollBarVisibility �� Orientation �� HorizontalOffset �� ScrollableWidth �� ViewportWidth, �� %ComputedHorizontalScrollBarVisibilityF  @System.Windows.Controls.DataVisualization.Charting.LineDataPoint  myDataPointStyle 	 Grid2M  GSystem.Windows.Controls.DataVisualization.Charting.Primitives.EdgePanel  MyEdgePanel>  8System.Windows.Controls.DataVisualization.Charting.Chart  MyChart  MyDockPanel  MyTextBlock  MyControlButton J� SnapsToDevicePixels �� Triggers�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      $localclr-namespace:QRS_Detector  5      1System&clr-namespace:System;assembly=mscorlib  5      �dv{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit  5      ~datavisrclr-namespace:System.Windows.Controls.DataVisualization;assembly=System.Windows.Controls.DataVisualization.Toolkit  5      �chartingprimitives�clr-namespace:System.Windows.Controls.DataVisualization.Charting.Primitives;assembly=System.Windows.Controls.DataVisualization.Toolkit  5      %U  &	        &	 !     &	 B     &	 c     &	 �     &	 �     &	 �     &	 �     7  7  7  7  7  7   &	 �    7  7   7  &		 &    7  7  &	
 �    7  7  &	 �    7  &	 2    7   &	 v    7   &	 �    7  7  7  7  7  7  7  7  7  &	 *    7  7   7  7  7  7  &	 �	    7  7  &	 b    7   7  7  &	 ,    7  &	 p    7   &	 �    &	     7  7  7  7  &	 �    7  &	 .    7  &	 8    7  7  �� 5	      $  #252526��6)   �� 5
      $  #2d2d2d��6+   �� 5      $  #38383c��6*   �� 5      $
  Gray��6&   �� 5      $  #84ceff��6&   �� 5      $  #68217a��6'   �� 5      $  White��6&   �� 5      " ��6   .�5   
   ��  � ��8   6(   �� 5   
    � ��8  6'   �� 5   
    � ��8  6'   �� 5   
    � �� 5      �� 5      " ��6"   .R�5      �� #��z��6   8�� $��1q�6n   ._�5      �� $��Center=�6+   $��Center=�6H   5      5      5       5   
   P� 5       �@J�IsMouseOver . . �5       ��  � ��8  6/   �� 5!       � ��8  6/   5"      5$      �� 5&      " ��6   .�5'   
   ��  � ��8   6(   �� 5(   
    � ��8  6'   �� 5)   
    � ��8  6'   5*      �� 5,      " ��6   .�5-   
   ��  � ��8   6(   �� 5.   
    � ��8  6'   5/      �� 51      " ��6   .�52   
   ��  � ��8   6(   �� 53   
    � ��8  6'   54      �� 56      " �6   .�57   
   ��  � ��8   6'   58      �� 5:      "   6#   .�5;   
   ��  � ��8   6'   5<      �� 5>      "  6)   .�5?   
   ��  � ��8   6'   5@      �� 5B      "  6   .�5C   
   ��  � ��8   6(   �� 5D   
    � ��8  6'   �� 5E   
    � ��8  6'    5F   
   P� 5G       �@ SortDirection 6/   Z� . �5H      ��  � ��# �  6/   �� 5I       � ��
 ���� 61   5J      ]� 5K       5L      �� 5M      	 �@J�IsMouseOver
 . 67   �� 5N      	 �@ SortDirection
 69   Z� ./�5P      ��  � ��8  6/   �� 5Q       � ��8  60   5R      ]� 5T       5U      �� 5V      	 �@J�IsMouseOver
 . 67   �� 5W      	 �@ SortDirection
 69   Z� ./�5Y      ��  � ��8  6/   �� 5Z       � ��8  60   5[      P� 5\       �@ SortDirection$ 	AscendingX�6/   . �5]      ��  � ��8  6/   5^      P� 5_       �@ SortDirection$ 
DescendingX�6/   . �5`      ��  � ��8  6/   5a      5c      �� 5e      "  6   .�5f   
   ��  � ��8   6(   �� 5g   
    � ��8  6'   �� 5h   
    � ��8  6'   �� 5i   
    �@ ColumnHeaderStyle8  6.   �� 5j   
    �@ RowStyle8  6%   �� 5k   
    �@ RowHeaderStyle8  6+    5l   
   P� 5m       �@ 
IsReadOnly . . �5n      ��  � ��
 �����64   5o      5q      �� 5s      " ��6%   .�5t   
   ��  � ��8   6'   �� 5u   
    � ��8  6'   �� 5v   
    � �� 6(   Z� �� 5w   
    �@��IsPressAndHoldEnabled .  69   �� 5x   
    �@��IsFlicksEnabled .  63    5y   
   P� 5z       �@J�IsMouseOver . . �5{      ��  �@J�Opacity$	 0.9Q�6,   5|      P� 5}       �@J�IsMouseOver .  . �5~      ��  �@J�Opacity$	 0.4Q�6,   5      5�      �� 5�      " ��6   .�5�   
   ��  � ��8   6'   �� 5�   
    �@�OverridesDefaultStyle . 62   �� 5�   
    � �� 5�      �� 5�      " ��6"   .R�5�      �  5�      �� 5�      $��* �6/   �� 5�      $
��Auto �6/    5�      �� 5�      $��* �6,   �� 5�      $
��Auto �6,   .+�5�      �� $��2��61   $��2��6E   �� 5�      $��PART_VerticalScrollBar��5�      8�� $��1��5�      # z 5�      # z 5�      # z 5�      #y�z 5�      �� 5�      $��PART_HorizontalScrollBar��5�      8�� $ 
Horizontal=�5�      $��1��5�      # z 5�      # z 5�      # z 5�      #y�z 5�      5�      5�      5�      5�      �� 5�      "  6   .�5�   
   ��  � ��8   6'   5�      �� 5�      " �6   .�5�   
   ��  � ��8   6'   �� 5�   
    �@�ShowGridLines . 6*   5�      �� 5�      "  6   5�      �� 5�      "  6   .�5�   
   ��  � ��8   6'   �� 5�   
    � ��8  6'   �� 5�   
    � �� 6(   Z� �� 5�   
    �@ ChartAreaStyle8  6+   �� 5�   
    �@ PlotAreaStyle8  6*   5�      �� 5�      " ]�6   .�5�   
   ��  � ��8   6'   5�      �� 5�      " ��6   .�5�   
   ��  � ��8   6'   �� 5�   
    � ��$ 14(�6%   �� 5�   
    � ��$
 Bold"�6'   �� 5�   
    � ��$ Myanmar Text)�6'   �� 5�   
    � ��$
 Left=�60   �� 5�   
    � ��$ Center=�6.   5�      �� 5�      " ��6   .�5�   
   ��  � �� 6'   Z� �� 5�   
    � ��8   6'   �� 5�   
    � ��8  6(   �� 5�   
    � ��$ 20��6"   �� 5�   
    � ��$ 20��6#   �� 5�   
    � �� 5�      �� 5�      " ��6"   .R�5�      �� #��z��6   #��z��6G   #��z��6s   ._�5�      �� #��z��6+   #��z��5�      #��z��68   # z 5�      ��. 6Q   5�       5�      P� 5�       �@J�IsMouseOver . . �5�      ��  � ��
 ��  �6;   �� 5�       � ��
 ��  �6;   �� 5�       �@�Cursor$
 Hand��67   5�      5�      5�      5�      5�      