---
uid: Home
title: Home
---
# PagedControl #

The @Manina.Windows.Forms.PagedControl winforms control houses a number of pages for grouping child controls. Only one page can be visible at a time. 

Pages can be added at design time, or at run-time through the @Manina.Windows.Forms.PagedControl.Pages property of the control. 

The user can navigate between pages by setting the @Manina.Windows.Forms.PagedControl.SelectedPage and @Manina.Windows.Forms.PagedControl.SelectedIndex properties. Or calling the @Manina.Windows.Forms.PagedControl.GoBack and @Manina.Windows.Forms.PagedControl.GoNext methods for sequental navigation.

When a page is switched, a number of events are fired by the control. Most important of these are the @Manina.Windows.Forms.PagedControl.PageValidating and the @Manina.Windows.Forms.PagedControl.PageChanging event. The latter allows the user to change the target page or to cancel the page change entirely.

The control also raises the @Manina.Windows.Forms.PagedControl.CreateUIControls and @Manina.Windows.Forms.PagedControl.UpdateUIControls events which allows the addition of user controls on it. Those controls could be used to create a paged wizard control, for example.

<div>![PagedControl in designer](./resources/images/PagedControl.designer.png)</div>
