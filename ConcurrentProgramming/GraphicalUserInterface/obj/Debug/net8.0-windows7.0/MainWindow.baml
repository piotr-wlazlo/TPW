   M S B A M L   `   `   `  ����i  eTP.ConcurrentProgramming.PresentationViewModel, Version=4.2.0.0, Culture=neutral, PublicKeyToken=null�uclr-namespace:TP.ConcurrentProgramming.Presentation.ViewModel;assembly=TP.ConcurrentProgramming.PresentationViewModel/TP.ConcurrentProgramming.Presentation.ViewModel  5   	   - )TP.ConcurrentProgramming.PresentationView:   4TP.ConcurrentProgramming.PresentationView.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   �vmuclr-namespace:TP.ConcurrentProgramming.Presentation.ViewModel;assembly=TP.ConcurrentProgramming.PresentationViewModel     5   	     � Title$5  /Presentation layer targeting reactive behaviour��5   	   $	��600��6A   $	��500��6N    � DataContext 5	      I   CTP.ConcurrentProgramming.Presentation.ViewModel.MainWindowViewModel  5
   
   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $	��Top=�6    �� Orientation$ 
Horizontal=�6*   $��Center=�6C   $��10, 10, 10, 0q�6`   $��0��6w   .+�5      �� $��start��6   $��10q�6F   $	��100��6R   $��35��6^   $��Center=�6j   $��Center=�6�   $��16(�6�   ��6%   �� *StartCommand+6�   �� 5      $
��stop��6   $��10q�6D   $	��100��6P   $��35��6\   $��Center=�6h   $��Center=�6�   $��16(�6�   ��6$   �� *StopCommand+6�   5      (� 5   
    (� Stretch$ Uniform=�6   $��1��6$   .��5      �� $��5q�6   
���� ��
�������$	��410��6U   $	��410��6b   $��Center=�6n   ._�5      �� ��6   �� *Balls+��5      �� 5      .R�5      �� $	��400��6%   $	��400��62   $��-20, -20, 0, 0q�6>   5      ��5!      �� 5"      .R�5#      � .+�5$   "   A� ��6*   �� *
Diameter+��6E   �� *
Diameter+��5%   &   �� 5&   *    �� 
TargetType" A�60    �� Triggers 5'   .   �� 5(   2    �� Binding 6>   �� *Mass+ �� Value 1.06W   .a�5)   6   �� 	 �� Property	 � ��
 �� Value

 �����6M   5*   3   �� 5+   2    6>   �� *Mass+ 2.56W   .a�5,   6   �� 	 � ��

 ��i��6M   5-   3   �� 5.   2    6>   �� *Mass+ 5.06W   .a�5/   6   �� 	 � ��

 ����6M   50   3   52   +   54   #   �� 55   "   $��Center=�56   *   $��Center=�57   *   
���   �$
��Bold"�59   *   ��55   ,   �� *ID+59   *   5:      5;      ��5=      �� 5>      " ��6    .�5?      �� 	 �@��Top
 6;   �� *Top+�� 5@      	 �@��Left
 6<   �� *Left+5A      5C      5D      5E      �� 5G   
   $��Bottom=�6   $ 
Horizontal=�6-   $��Center=�6F   $��10, 0, 10, 10q�6c   $��2��6z   .+�5H      �� $��
add a ball��6   $��10q�6M   $	��125��6Y   $��35��6e   $��Center=�6q   $��Center=�6�   $��16(�6�   ��6*   �� *AddBallCommand+6�   �� 5I      $��remove a ball��6   $��10q�6S   $	��125��6_   $��35��6k   $��Center=�6w   $��Center=�6�   $��16(�6�   ��6-   �� *RemoveBallCommand+6�   �� 5K      $
��Left=�6   $ Vertical=�6/   $��Right=�6F   $��10q�6b   .+�5L      �� $��number of balls��6   $��Center=�63   $��5q�6N   $��16(�6Y   �� 5M      $��50��6Z   $��25��6e   $��5q�6q    �� TextAlignment$ Center=�6|   $��16(�6�   ��6   �� *	BallCount+ �� UpdateSourceTrigger$ PropertyChanged=�6�   5N      5O      5P      5Q      