   M S B A M L   `   `   `  ����A  =Sudoku, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Sudoku.PagesSudoku.Pages  5          Sudoku.Pages.RandomSudoku   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,localclr-namespace:Sudoku.Pages     5        N� Title$  RandomSudoku��5	      .��5      � -   5         �Name$ mainGrid��5      
�������.+�5   
   �� -   5         ��Name$ Extract��5      
���**��$	��200��5      $	��100��5      $��Right=�5      $	��Top=�5      $��	0,10,10,0q�5       �� TextAlignment$ Center=�5      $��15(�5      
�������$y�Hidden=�5      6*   �� 5   
   -   5         ��Name$ stackPanelViz��5      
������ $	��222��5      $
��Left=�5      $	��Top=�5      $��10q�5      $y�Visible=�5      .+�5      �� $��Center=�6   $��0,0,0,5q�5       $��40��5!      $	��200��5"      
�������
���**��$��Number of steps:��5%      $��25(�5&      6)   �� 5'      -   5         ��Name$ numberOfStepsBox��50      $��Center=�5'      $��0,0,0,5q�5(      $��40��5)      $��50��5*      
�������
���**��$��25(�5-       �� TextAlignment$ Center=�5.      $��0��5/      50      �� 51      -   5         ��Name$ visualButton��59      $��Center=�51      $��40��52      $	��150��53      
�������
���**��$��Visualization��56      $��25(�57      
���   �5:   )   5;      �� 5<   
   -   5        $ solvedButton��5H      $
��left=�5<      $��Bottom=�5=      $��	10,0,0,10q�5>      $��40��5?      $	��150��5@      
�������
���**��$��Solve��5C      $��25(�5D      
���   �$y�Hidden=�5G      5H   )   �� 5I   
   -   5        $��right=�5I      $��Bottom=�5J      $��	0,0,10,10q�5K      $��40��5L      $	��150��5M      
�������
���**��$��	Main Menu��5P      $��25(�5Q      
���   �5S   $   5T      5U      