[![License](http://img.shields.io/github/license/oozcitak/pagedcontrol.svg?style=flat-square)](https://opensource.org/licenses/MIT)
[![Nuget](https://img.shields.io/nuget/v/PagedControl.svg?style=flat-square)](https://www.nuget.org/packages/PagedControl)
[![Build status](https://ci.appveyor.com/api/projects/status/u00t9bmcisqit8q7?svg=true)](https://ci.appveyor.com/project/oozcitak/pagedcontrol)

PagedControl is a .NET control with multiple pages for hosting child controls. The control is devoid of any decorations except an optional border. It can be used as a paged panel control. Or, the control can be extended for a specific purpose, like a [tab control](https://github.com/oozcitak/TabControl) or a [wizard control](https://github.com/oozcitak/WizardControl).

![PagedControl in Designer](https://raw.githubusercontent.com/wiki/oozcitak/PagedControl/PagedControl.designer.png)

The control has full designer support for adding/removing pages and dragging child controls.

# Installation #

If you are using [NuGet](https://nuget.org/) you can install the assembly with:

`PM> Install-Package PagedControl`

# Properties #

Following public properties are available.

| Name | Type | Description |
|------|------|-------------|
| SelectedPage      | Page                           | Gets or sets the currently selected page. |
| SelectedIndex     | int                            | Gets or sets the index of the currently selected page. |
| Pages             | PagedControl.PageCollection    | Gets the collection of pages. |
| CanGoBack         | bool                           | Gets whether the control can navigate to the previous page. |
| CanGoNext         | bool                           | Gets whether the control can navigate to the next page. |
| DisplayRectangle  | System.Drawing.Rectangle       | Gets the client rectangle where pages are located. Deriving classes can override this property to modify page bounds. [WizardControl](https://github.com/oozcitak/WizardControl), for example, overrides this property to provide space for its navigation buttons. |

# Methods #

Following public methods are available.

| Name | Description |
|------|-------------|
| GoBack() | Navigates to the previous page if possible. |
| GoNext() | Navigates to the next page if possible. |

# Events #

Following events are raised by the control:

| Name | Event Argument | Description |
|------|----------------|-------------|
| PageChanging | PagedControl.PageChangingEventArgs   | Occurs before the selected page changes. The event arguments contains references to the currently selected page and the page to become selected. It is possible to make the control navigate to a different page by setting the `NewPage` property of the event arguments, or to cancel navigation entirely by setting `Cancel = true` while handling the event. |
| PageChanged  | PagedControl.PageChangedEventArgs    | Occurs after the selected page changes. The event arguments contains references to the currently selected page and the previous selected page. |
| PageAdded           | PagedControl.PageEventArgs           | Occurs after a new page is added to the page collection. The event arguments contains a reference to the new page. |
| PageRemoved         | PagedControl.PageEventArgs           | Occurs after an existing page is removed from the page collection. The event arguments contains a reference to the removed page. |
| PageValidating      | PagedControl.PageValidatingEventArgs | Occurs before the selected page changes and it needs to be validated. The event arguments contains a reference to the currently selected page. By setting `Cancel = true` while handling the event, the validation stops and the selected page is not changed. |
| PageValidated       | PagedControl.PageEventArgs           | Occurs before the selected page changes and after it is successfully validated. The event arguments contains a reference to the currently selected page. |
| PageHidden          | PagedControl.PageEventArgs           | Occurs before the selected page changes and after the currently selected page is hidden. The event arguments contains a reference to the page. |
| PageShown           | PagedControl.PageEventArgs           | Occurs before the selected page changes and the page to become selected is shown. The event arguments contains a reference to the page. |
| PagePaint           | PagedControl.PagePaintEventArgs      | Occurs when a page is needed to be painted. The control paints the background of the pages by default. |
| CreateUIControls    | PagedControl.CreateUIControlsEventArgs | Gets the collection of user interface controls. The control creates no UI controls by default but deriving classes can handle this event to provide additional UI controls. [WizardControl](https://github.com/oozcitak/WizardControl), for example, uses this event to supply its navigation buttons. |
| UpdateUIControls    | System.EventArgs                     | Occurs when the visual states of the user interface controls are needed to be updated. If any custom UI controls are created with the `CreateUIControls` event, visual states of those controls should be handled in this event. |
