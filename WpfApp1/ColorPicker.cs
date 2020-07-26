using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfApp1"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfApp1;assembly=WpfApp1"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:ColorPicker/>
    ///
    /// </summary>
    
    [TemplatePart(Name="PART_RedSlider", Type = typeof(RangeBase))]
    [TemplatePart(Name="PART_BlueSlider", Type = typeof(RangeBase))]
    [TemplatePart(Name="PART_GreenSlider", Type = typeof(RangeBase))]
    [TemplatePart(Name="PART_PreviewBrush", Type = typeof(SolidColorBrush))]
    public class ColorPicker : Control
    {
        public static DependencyProperty ColorProperty;
        public static DependencyProperty RedProperty;
        public static DependencyProperty GreenProperty;
        public static DependencyProperty BlueProperty;

        public static readonly RoutedEvent ColorChangeEvent;
        public event RoutedPropertyChangedEventHandler<Color> ColorChange
        {
            add { AddHandler(ColorChangeEvent, value); }
            remove { RemoveHandler(ColorChangeEvent, value); }
        }

        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));

            ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(ColorPicker),
                new FrameworkPropertyMetadata(Colors.Black, OnColorChanged));

            RedProperty = DependencyProperty.Register("Red", typeof(byte), typeof(ColorPicker),
                new FrameworkPropertyMetadata(OnColorRGBChanged));
            GreenProperty = DependencyProperty.Register("Green", typeof(byte), typeof(ColorPicker),
                new FrameworkPropertyMetadata(OnColorRGBChanged));
            BlueProperty = DependencyProperty.Register("Blue", typeof(byte), typeof(ColorPicker),
                new FrameworkPropertyMetadata(OnColorRGBChanged));

            ColorChangeEvent = EventManager.RegisterRoutedEvent("ColorChanged", RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<Color>), 
                typeof(ColorPicker));

            
        }

        public static RoutedUICommand _customCmd;
        public static RoutedUICommand CustomCmd => _customCmd;

        public ColorPicker()
        {
            CommandBinding binding =
                new CommandBinding(ApplicationCommands.Undo, UndoCommand_Executed, UndoCommand_CanExecute);
            this.CommandBindings.Add(binding);

            if (_customCmd == null)
            {
                _customCmd = new RoutedUICommand("CustomCmd", "CustomCmd", typeof(ColorPicker));
                CommandBinding bindingCustom =
                    new CommandBinding(CustomCmd, CustomCmd_Executed, CustomCmd_CanExecute); 
                this.CommandBindings.Add(bindingCustom);
            }
            
        }

        public static void CustomCmd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public static void CustomCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private Color? previousColor;

        private void UndoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = previousColor.HasValue;
        }

        private void UndoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (previousColor.HasValue)
                this.Color = (Color) previousColor.Value;
        }


        private static void OnColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var newColor = (Color) e.NewValue;
            ColorPicker colorPicker = (ColorPicker) sender;
            colorPicker.Red = newColor.R;
            colorPicker.Green = newColor.G;
            colorPicker.Blue = newColor.B;

            var oldColor = (Color) e.OldValue;
            RoutedPropertyChangedEventArgs<Color> args = new RoutedPropertyChangedEventArgs<Color>(oldColor, newColor);
            args.RoutedEvent = ColorPicker.ColorChangeEvent;

            colorPicker.RaiseEvent(args);

            colorPicker.previousColor = (Color) e.OldValue;
        }
        
        private static void OnColorRGBChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker colorPicker = (ColorPicker) sender;
            Color color = colorPicker.Color;

            if (e.Property == RedProperty)
                color.R = (byte) e.NewValue;
            else if (e.Property == GreenProperty)
                color.G = (byte) e.NewValue;
            else if (e.Property == BlueProperty)
                color.B = (byte) e.NewValue;

            colorPicker.Color = color;
        }

        public Color Color
        {
            get => (Color) GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
        
        public byte Red
        {
            get => (byte) GetValue(RedProperty);
            set => SetValue(RedProperty, value);
        }
        
        public byte Green
        {
            get => (byte) GetValue(GreenProperty);
            set => SetValue(GreenProperty, value);
        }

        public byte Blue
        {
            get => (byte) GetValue(BlueProperty);
            set => SetValue(BlueProperty, value);
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            RangeBase sliderRed = GetTemplateChild("PART_RedSlider") as RangeBase;
            if (sliderRed != null)
            {
                Binding binding = new Binding("Red");
                binding.Source = this;
                binding.Mode = BindingMode.TwoWay;
                sliderRed.SetBinding(RangeBase.ValueProperty, binding);
            }

            RangeBase sliderGreen = GetTemplateChild("PART_GreenSlider") as RangeBase;
            if (sliderGreen != null)
            {
                Binding binding = new Binding("Green");
                binding.Source = this;
                binding.Mode = BindingMode.TwoWay;
                sliderGreen.SetBinding(RangeBase.ValueProperty, binding);
            }

            RangeBase sliderBlue = GetTemplateChild("PART_BlueSlider") as RangeBase;
            if (sliderBlue != null)
            {
                Binding binding = new Binding("Blue");
                binding.Source = this;
                binding.Mode = BindingMode.TwoWay;
                sliderBlue.SetBinding(RangeBase.ValueProperty, binding);
            }

            // ...


            SolidColorBrush brush = GetTemplateChild("PART_PreviewBrush") as SolidColorBrush;
            if (brush != null)
            {
                Binding binding = new Binding("Color");
                binding.Source = brush;
                binding.Mode = BindingMode.OneWayToSource;
                this.SetBinding(ColorPicker.ColorProperty, binding);
            }
        }



    }
}
