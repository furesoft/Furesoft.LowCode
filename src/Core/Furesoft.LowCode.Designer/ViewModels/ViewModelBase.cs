﻿using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Furesoft.LowCode.Editor.Model;

namespace Furesoft.LowCode.Designer.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    [Browsable(false)]
    public IDrawingNode Drawing { get; set; }
}