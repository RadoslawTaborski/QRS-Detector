   M S B A M L   `   `   `  ����l  hSystem.Windows.Controls.DataVisualization.Toolkit, Version=4.0.0.0, Culture=neutral, PublicKeyToken=null�{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting  5   	   G CQRS_Detector, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:QRS_DetectorQRS_Detector 5   	      QRS_Detector.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   �dv{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit     5   	   ,localclr-namespace:QRS_Detector     5   	     � WindowStyle$
  None=�5   	    � Title$ QRS Detector��5
   	   $	��600��6   $
��1200��6+    � AllowsTransparency .  � 
ResizeMode$ CanResizeWithGrip=�5   	    � Icon$ Img/logo.ico��5   	    � 	Resources 5      ��%    .��5      � $��0,0,0,0q�6    
  MyGrid#��[  6    � RowDefinitions 5   
   �� 5      �� 5      $	��140 �6    � ColumnDefinitions 5   
   �� 5      $	��220 �6   �� 5      .+�5   
   ]� ��. $��2��6)   .+�5      ��   MyBorder#��[ 6   $��1q�68   $	��206��6L   $��10,40,0,140q�6X   {�. ._�5      &   System.Windows.Controls.DataGrid  -   5          Name$ dataGrid��5   R     
MyDataGrid#��[ 6   	   
IsReadOnly	 . $��10,65,10,60q�6e   "
   VerticalScrollBarVisibility$
 Hidden=�6z   $   HorizontalScrollBarVisibility$ Disabled=�6�      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Q [s]��6-      Width5  /System.Windows.Controls.DataGridLengthConverter$ 62 6R   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6<   �� *Q+6R     5      $ R [s]��6-   $ 62 6R    6<   �� *R+6R     5      $ S [s]��6-   $ 62 6R    6<   �� *S+6R   5       5!      5"      �� 5#   
   #��[ 6   $��1q�64   $��12,2,170,10q�6H   $��1��6]   $��2��6j   6�   �� 5$   
   #��[ 6   $��1q�64   $��	0,40,10,0q�6H   $��1��6[   �� 5%   
   #��[ 6   $��1q�64   $��	0,2,10,10q�6H   $��1��6[   $��1��6h   $��Right=�6x   $	��158��6�   �� 5&   
   -   5         ��Name$ lSave��5&        MyButton#��[ 6    $��Zapisz��6B   $��Right=�6S   $��0,80,180,30q�6o   $��92��6�   $��1��6�   $��1��6�   �� 5'   
   -   5        $ lLoad��5'      #��[ 6    $��Wczytaj��6B   $��Right=�6T   $��0,22,180,88q�6p   $��92��6�   $��1��6�   $��1��6�   �� 5(   
   -   5        $ Button2��5(      #��[ 6"   $��Detekcja��6D   $��1��6m   $��Right=�6}   $��
0,80,22,30q�6�   $	��133��6�   $��1��6�   �� 5)   
   -   5        $ Button1��5)      #��[ 6"   $��Rysuj��6D   $��1��6j   $��
0,22,22,88q�6z   $��1��6�   $��Right=�6�   $	��133��6�   �� 5*   
   -   5         ��Name$ label��5*        MyLabel#��[ 6   $��Wczytaj z pliku: ��6@   $��Right=�6\   $��
25,22,0,92q�6   $��1��6�   $
��Left=�6�   $��99��6�   �� 5+   
   -   5        $ label2��5+      #��[ 6    $��Zapisz do pliku: ��6A   $��Right=�6]   $��	25,0,0,33q�6�   $��1��6�   $��26��6�   $��Bottom=�6�   $
��Left=�6�   $��99��6�   �� 5,   
   -   5         ��Name$ tbPath��5,        	MyTextBox#��[ 6"    �� MaxLines$ 1��6E   $��124,22,277,88q�6S   $��1��6j    �� TextWrapping$
 Wrap=�6w   $�� ��6�   {�.  $��2��6�   $��Center=�6�   $��12(�6�   �� 5-   
   -	   5        $ tbPath2��5-      #��[ 6#   $ 1��6F   $��124,80,277,30q�6T   $��1��6k   $
 Wrap=�6x   $�� ��6�   {�.  $��2��6�   $��Center=�6�   $��12(�6�   �� 5.   
   -
   5        $ lHeader��5.      #��[ 6!   $��
Informacje��6B   $��	12,47,5,0q�6W   $��15(�6j   $��55��6y   $	��Top=�6�   $��Center=�6�   $��Center=�6�   $
��Bold"�6�   �� 5/   
   -   5         ��Name$ svChart��5/        MyScrollViewer#��[ 6(   $
��Auto=�6P   $��Disabled=�6u   $��1��6�   $��	1,41,11,1q�6�   
�������.��50      >   8System.Windows.Controls.DataVisualization.Charting.Chart  -   5          Name$ Chart1��50        MyChart#��[ 6'      Title$  ��6H   $
��Bold"�6R   51      �� 52   
   #��[ 6   $��1q�64   $��	0,50,22,0q�6H   $��1��6[   $��Right=�6k   $	��146��6�   $��48��6�   $	��Top=�6�   �� 53   
   -   5        $ rHeader��53      #��[ 6!   $	��EKG��6B   $��	0,47,10,0q�6P   $��15(�6c   $��55��6r   $	��Top=�6~   $��Center=�6�   $��Center=�6�   $
��Bold"�6�   $��1��6�   �� 54   
   -   5        $
 Plus��54      #��[ 6   $��+��6A   $��1��6`   $��	0,54,99,0q�6p   $��40��6�   $	��Top=�6�   $��Right=�6�   $��65��6�   �� 55   
   -   5        $ Minus��55      #��[ 6    $��-��6B   $��1��6b   $��	0,54,26,0q�6r   $��Right=�6�   $��70��6�   $��40��6�   $	��Top=�6�   ]� 56   
   -   5         ]�Name$
 Dock��56      $��2��6"     MyDockPanel#��[ 66   ��.  $��30��6q   $	��Top=�6}   .+�57      ]� -   5        $ TitleBar��57      #��[ 6*   ��6n   �� *ActualWidth+ �� ElementName$
 Dock��.+�58      �� $��Img/logo.png��6   $
��Left=�6.   $��26��6D   $��26��6O   $��13,0,0,0q�6[   �� 59      -   5        $ CloseButton��59       	 MyControlButton#��[	 6.   $��Right=�6W   $
��Left=�6�   $��0,3,10,3q�6�   ��5:      �� 5;       �� ImageSource$ Img/close.png��6%   5=      �� 5>      -   5        $ 	MaxButton��5>      #��[	 6,   $��Right=�6U   $
��Left=�6�   $��0,3,5,3q�6�   ��5?      �� 5@      $ Img/max.png��6%   5B      �� 5C      -   5        $ 	MinButton��5C      #��[	 6,   $��Right=�6U   $
��Left=�6�   $��0,3,5,3q�6�   ��5D      �� 5E      $ Img/minimize.png��6%   5G      �� 5I       
 MyTextBlock#��[
 6   $��	32,7,70,0q�6A   $
��Left=�6T   $��23��6j   QRS Detector6�   5J      5K      5L      5M      