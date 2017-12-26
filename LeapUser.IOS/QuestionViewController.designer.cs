// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace LeapUser
{
    [Register ("QuestionViewController")]
    partial class QuestionViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton buttonA { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton buttonB { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton buttonC { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton buttonD { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem buttonSubmit { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel textOptionA { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel textOptionB { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel textOptionC { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel textOptionD { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel textQuestion { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UINavigationItem titleSession { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UINavigationBar toolbarSession { get; set; }

        [Action ("ButtonSubmit_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ButtonSubmit_Activated (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (buttonA != null) {
                buttonA.Dispose ();
                buttonA = null;
            }

            if (buttonB != null) {
                buttonB.Dispose ();
                buttonB = null;
            }

            if (buttonC != null) {
                buttonC.Dispose ();
                buttonC = null;
            }

            if (buttonD != null) {
                buttonD.Dispose ();
                buttonD = null;
            }

            if (buttonSubmit != null) {
                buttonSubmit.Dispose ();
                buttonSubmit = null;
            }

            if (textOptionA != null) {
                textOptionA.Dispose ();
                textOptionA = null;
            }

            if (textOptionB != null) {
                textOptionB.Dispose ();
                textOptionB = null;
            }

            if (textOptionC != null) {
                textOptionC.Dispose ();
                textOptionC = null;
            }

            if (textOptionD != null) {
                textOptionD.Dispose ();
                textOptionD = null;
            }

            if (textQuestion != null) {
                textQuestion.Dispose ();
                textQuestion = null;
            }

            if (titleSession != null) {
                titleSession.Dispose ();
                titleSession = null;
            }

            if (toolbarSession != null) {
                toolbarSession.Dispose ();
                toolbarSession = null;
            }
        }
    }
}