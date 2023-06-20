# MaterialSkin 2 for .NET WinForms with RTL support

Theming .NET WinForms, C# or VB.Net, to Google's Material Design Principles.

> This project is ACTIVE (With some long pauses in between, but I still read every issue and check every PR)
>
![home](https://raw.githubusercontent.com/baqeryan/MaterialSkin/master/MaterialSkinExample.RTL/Resources/MaterialSkinExample.RTL.gif)

## Nuget Package

A nuget package version is available [here](https://www.nuget.org/packages/MaterialSkin.2.RTL/)

Or simply search for MaterialSkin.2.RTL on the **Nuget Package Manager** inside Visual Studio

## WIKI Available!

But there's not much in there for now, please contribute if you can. :smile:

You can access it [here](https://github.com/baqeryan/MaterialSkin/wiki)

## Current state of the MaterialSkin components

| Component                    | Supported   | Disabled mode | Animated | RTL supported |
| ---------------------------- | :-------:   | :-----------: | :------: | :-----------: |
| Backdrop                     |  **No**     |       -       |    -     |    -          |
| Banner                       |  **No**     |       -       |    -     |    -          |
| Buttons                      |    Yes      |      Yes      |   Yes    |   Yes         |
| Cards                        |    Yes      |      N/A      |   N/A    |   Yes         |
| Check Box                    |    Yes      |      Yes      |   Yes    |   Yes         |
| Check Box List               |    Yes      |      Yes      |   Yes    |   Yes         |
| Chips                        |  **No**     |       -       |    -     |    -          |
| Combobox                     |    Yes      |      Yes      |   Yes    |   Yes         |
| Context Menu                 |    Yes      |      Yes      |   Yes    |   Yes         |
| Date Picker                  |  **No**     |       -       |    -     |    -          |
| Dialog                       |    Yes      |      N/A      |  **No**  |   Yes         |
| Divider                      |    Yes      |      N/A      |   N/A    |   N/A         |
| Drawer                       |    Yes      |      N/A      |   Yes    |   Yes         |
| Expansion Panel              |    Yes      |      Yes      |  **No**  |   Yes         |
| Flexible Dialog (big)        |    Yes      |      Yes      |   N/A    |   Yes         |
| FAB - Floating Action Button |    Yes      |      Yes      |   Yes    |   Yes         |
| Label                        |    Yes      |      Yes      |   N/A    |   Yes         |
| ListBox                      |    Yes      |      Yes      |   N/A    |   Yes         |
| ListView                     |    Yes      |    **No**     |   N/A    |   N/A         |
| Progress Bar                 |  _Partial_  |    **No**     |  **No**  |   N/A         |
| Radio Button                 |    Yes      |      Yes      |   Yes    |   Yes         |
| Text field                   |    Yes      |      Yes      |   Yes    |   Yes         |
| Sliders                      |    Yes      |      Yes      |  **No**  |   N/A         |
| SnackBar                     |    Yes      |      N/A      |   Yes    |   Yes         |
| Switch                       |    Yes      |      Yes      |   Yes    |   Yes         |
| Tabs                         |    Yes      |      N/A      |   Yes    |   Yes         |
| Time Picker                  |  **No**     |       -       |    -     |    -          |
| Tooltips                     |  **No**     |       -       |    -     |    -          |

All supported components have a dark theme


## Contributing

Thanks for taking the time to contribute!  :+1:

If you have any issues please open an issue; have an improvement? open a pull request.

> - This project was heavily updated by [@leocb](https://github.com/leocb/MaterialSkin)
> - Currently it's kept alive by [@orapps44](https://github.com/orapps44/MaterialSkin)
> - forked from [@donaldsteele](https://github.com/donaldsteele/MaterialSkin)
> - and he forked it from the original [@IgnaceMaes](https://github.com/IgnaceMaes/MaterialSkin)

## Contributors

Thank you to all the people who have already contributed to MaterialSkin 2 !

<a href="https://github.com/leocb/MaterialSkin/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=leocb/MaterialSkin" />
</a>


---

## Implementing MaterialSkin 2 in your application

### 1. Add the library to your project

There are a few methods to add this lib:

#### The Easy way

Search for MaterialSkin.2 on the Nuget Package manager inside VisualStudio and add it to your project.

#### Manual way

Download the precompiled DLL available on the releases section and add it as a external reference on your project.

#### Compile from the latest master

Clone the project from GitHub, then add the MaterialSkin.csproj to your own solution, then add it as a project reference on your project.
  
### 2. Add the MaterialSkin components to your ToolBox

Simply drag the MaterialSkin.dll file into your IDE's ToolBox and all the controls should be added there.

### 3. Inherit from MaterialForm

Open the code behind your Form you wish to skin. Make it inherit from MaterialForm rather than Form. Don't forget to put the library in your imports, so it can find the MaterialForm class!
  
#### C# (Form1.cs)

```cs
public partial class Form1 : MaterialForm
```
  
#### VB.NET (Form1.Designer.vb)

```vb
Partial Class Form1
  Inherits MaterialSkin.Controls.MaterialForm
```
  
### 4. Initialize your colorscheme

Set your preferred colors & theme. Also add the form to the manager so it keeps updated if the color scheme or theme changes later on.

#### C# (Form1.cs)

```cs
public Form1(RightToLeft RightToLeft = RightToLeft.Yes) : base(RightToLeft)
{
    InitializeComponent();

    var materialSkinManager = MaterialSkinManager.Instance;
    materialSkinManager.AddFormToManage(this);
    materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
    materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
}
```

#### VB.NET (Form1.vb)

```vb
Imports MaterialSkin

Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim SkinManager As MaterialSkinManager = MaterialSkinManager.Instance
        SkinManager.AddFormToManage(Me)
        SkinManager.Theme = MaterialSkinManager.Themes.LIGHT
        SkinManager.ColorScheme = New ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE)
    End Sub
End Class
```

---

## Material Design in WPF

If you love .NET and Material Design, you should definitely check out [Material Design Xaml Toolkit](https://github.com/ButchersBoy/MaterialDesignInXamlToolkit) by ButchersBoy. It's a similar project but for WPF instead of WinForms.

---

## Images

*A simple demo interface with MaterialSkin components.*
![home](https://raw.githubusercontent.com/baqeryan/MaterialSkin/master/MaterialSkinExample.RTL/Resources/demo-1.png)

*The MaterialSkin Drawer (menu).*
![drawer](https://raw.githubusercontent.com/baqeryan/MaterialSkin/master/MaterialSkinExample.RTL/Resources/demo-2.png)

*Every MaterialSkin button variant - this is 1 control, 3 properties*
![buttons](https://raw.githubusercontent.com/baqeryan/MaterialSkin/master/MaterialSkinExample.RTL/Resources/demo-3.png)

*The MaterialSkin checkboxes, radio and Switch.*
![selection](https://raw.githubusercontent.com/baqeryan/MaterialSkin/master/MaterialSkinExample.RTL/Resources/demo-4.png)

*Material skin textfield*
![text](https://raw.githubusercontent.com/baqeryan/MaterialSkin/master/MaterialSkinExample.RTL/Resources/demo-5.png)

*Table control*
![table](https://raw.githubusercontent.com/baqeryan/MaterialSkin/master/MaterialSkinExample.RTL/Resources/demo-6.png)

*Progress bar*
![progress bar](https://raw.githubusercontent.com/baqeryan/MaterialSkin/master/MaterialSkinExample.RTL/Resources/demo-7.png)

*Cards*
![cards](https://raw.githubusercontent.com/baqeryan/MaterialSkin/master/MaterialSkinExample.RTL/Resources/demo-8.png)

*List Box*
![listbox](https://raw.githubusercontent.com/baqeryan/MaterialSkin/master/MaterialSkinExample.RTL/Resources/demo-9.png)

*Expansion Panel*
![expansion](https://raw.githubusercontent.com/baqeryan/MaterialSkin/master/MaterialSkinExample.RTL/Resources/demo-10.png)

*Label*
![label](https://raw.githubusercontent.com/baqeryan/MaterialSkin/master/MaterialSkinExample.RTL/Resources/demo-11.png)

*MaterialSkin using a custom color scheme.*
![custom](https://user-images.githubusercontent.com/77468294/119881411-8e7e2f80-bf2d-11eb-9fa3-883eceabfadc.png)

*FlexibleMaterial Messagebox*
![messagebox](https://user-images.githubusercontent.com/8310271/66238105-25e59d80-e6cd-11e9-88c9-5a21ceae1a5a.png)
