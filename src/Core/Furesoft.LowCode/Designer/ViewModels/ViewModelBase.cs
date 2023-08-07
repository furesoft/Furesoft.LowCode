﻿using System.ComponentModel;
using Furesoft.LowCode.Editor.Model;

namespace Furesoft.LowCode.Designer.ViewModels;

public class ViewModelBase : ObservableObject
{
    [Browsable(false)] public IDrawingNode Drawing { get; set; }
}