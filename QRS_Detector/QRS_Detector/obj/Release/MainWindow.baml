   M S B A M L   `   `   `  ���� l  hSystem.Windows.Controls.DataVisualization.Toolkit, Version=4.0.0.0, Culture=neutral, PublicKeyToken=null�{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting  G CQRS_Detector, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:QRS_DetectorQRS_Detector    QRS_Detector.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     �dv{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit     ,localclr-namespace:QRS_Detector       � WindowStyle$
  None=� � Title$ QRS Detector��$	��600��$
��1200�� � AllowsTransparency .  � 
ResizeMode$ CanResizeWithGrip=� � Icon$ Img/logo.ico�� � 	Resources ��%    .��� $��0,0,0,0q� 
  MyGrid#��[   � RowDefinitions �� �� $	��140 � � ColumnDefinitions �� $	��220 ��� .+�]� ��. $��2��.+���   MyBorder#��[ $��1q�$	��206��$��10,40,0,140q�{�. ._�&   System.Windows.Controls.DataGrid  -     Name$ dataGrid��  
MyDataGrid#��[ 	   
IsReadOnly	 . $��10,65,10,60q�"
   VerticalScrollBarVisibility$
 Hidden=�$   HorizontalScrollBarVisibility$ Disabled=�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Q [s]��   Width5  /System.Windows.Controls.DataGridLengthConverter$ 62 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Q+  $ R [s]��$ 62  �� *R+  $ S [s]��$ 62  �� *S+�� #��[ $��1q�$��12,2,170,10q�$��1��$��2���� #��[ $��1q�$��	0,40,10,0q�$��1���� #��[ $��1q�$��	0,2,10,10q�$��1��$��1��$��Right=�$	��158���� -    ��Name$ lSave��  MyButton#��[ $��Zapisz��$��Right=�$��0,80,180,30q�$��92��$��1��$��1���� -   $ lLoad��#��[ $��Wczytaj��$��Right=�$��0,22,180,88q�$��92��$��1��$��1���� -   $ Button2��#��[ $��Detekcja��$��1��$��Right=�$��
0,80,22,30q�$	��133��$��1���� -   $ Button1��#��[ $��Rysuj��$��1��$��
0,22,22,88q�$��1��$��Right=�$	��133���� -    ��Name$ label��  MyLabel#��[ $��Wczytaj z pliku: ��$��Right=�$��
25,22,0,92q�$��1��$
��Left=�$��99���� -   $ label2��#��[ $��Zapisz do pliku: ��$��Right=�$��	25,0,0,33q�$��1��$��26��$��Bottom=�$
��Left=�$��99���� -    ��Name$ tbPath��  	MyTextBox#��[  �� MaxLines$ 1��$��124,22,277,88q�$��1�� �� TextWrapping$
 Wrap=�$�� ��{�.  $��2��$��Center=�$��12(��� -	   $ tbPath2��#��[ $ 1��$��124,80,277,30q�$��1��$
 Wrap=�$�� ��{�.  $��2��$��Center=�$��12(��� -
   $ lHeader��#��[ $��
Informacje��$��	12,47,5,0q�$��15(�$��55��$	��Top=�$��Center=�$��Center=�$
��Bold"��� -    ��Name$ svChart��  MyScrollViewer#��[ $
��Auto=�$��Disabled=�$��1��$��	1,41,11,1q�
�������.��>   8System.Windows.Controls.DataVisualization.Charting.Chart  -     Name$ Chart1��  MyChart#��[    Title$  ��$
��Bold"��� #��[ $��1q�$��	0,50,22,0q�$��1��$��Right=�$	��146��$��48��$	��Top=��� -   $ rHeader��#��[ $	��EKG��$��	0,47,10,0q�$��15(�$��55��$	��Top=�$��Center=�$��Center=�$
��Bold"�$��1���� -   $
 Plus��#��[ $��+��$��1��$��	0,54,99,0q�$��40��$	��Top=�$��Right=�$��65���� -   $ Minus��#��[ $��-��$��1��$��	0,54,26,0q�$��Right=�$��70��$��40��$	��Top=�]� -    ]�Name$
 Dock��$��2��  MyDockPanel#��[ ��.  $��30��$	��Top=�.+�]� -   $ TitleBar��#��[ ���� *ActualWidth+ �� ElementName$
 Dock��.+��� $��Img/logo.png��$
��Left=�$��26��$��26��$��13,0,0,0q��� -   $ CloseButton�� 	 MyControlButton#��[	 $��Right=�$
��Left=�$��0,3,10,3q�����  �� ImageSource$ Img/close.png���� -   $ 	MaxButton��#��[	 $��Right=�$
��Left=�$��0,3,5,3q����� $ Img/max.png���� -   $ 	MinButton��#��[	 $��Right=�$
��Left=�$��0,3,5,3q����� $ Img/minimize.png����  
 MyTextBlock#��[
 $��	32,7,70,0q�$
��Left=�$��23��QRS Detector