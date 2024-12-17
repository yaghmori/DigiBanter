using MudBlazor;

namespace ParsLinks.Admin.Themes
{
    public static class AppThemes
    {


        private static LayoutProperties DefaultLayoutProperties = new LayoutProperties()
        {
            DefaultBorderRadius = "8px",


        };

        public static MudTheme DefaultTheme = new MudTheme()
        {
            Typography = new Typography
            {

                Default = new Default
                {
                    FontSize = "12px",
                    FontFamily = ["roboto", "inter",]
                },
                Body1 = new Body1
                {
                    FontSize = "12px",
                    FontFamily = ["roboto", "inter",]

                },
                Button = new Button
                {
                    FontSize = "13px",
                    TextTransform = string.Empty,
                    FontFamily = ["roboto", "inter",]



                },
                H6 = new H6
                {
                    FontSize = "17px",
                    FontFamily = ["roboto", "inter",]

                },
                H5 = new H5
                {
                    FontSize = "18px",
                    FontFamily = ["roboto", "inter",]


                },
                H4 = new H4
                {
                    FontSize = "19px",
                    FontFamily = ["roboto", "inter",]

                },
                H3 = new H3
                {
                    FontSize = "20px",
                    FontFamily = ["roboto", "inter",]

                },
                H2 = new H2
                {
                    FontSize = "21px",
                    FontFamily = ["roboto", "inter",]

                },
                H1 = new H1
                {
                    FontSize = "22px",
                    FontFamily = ["roboto", "inter",]

                }
            },
            PaletteLight = new PaletteLight()
            {
                TableLines = "#F1F1F4",

                TableStriped = "#FCFCFC",


                //Primary = "#2464f0",             //iphoneBlue -- "#2464f0"




                //Background = "#F7F7F9", //Vuex : "#F8F7FA", //Metronic : "#FCFCFC",  //Default: "#F9F9F9",             //iPhone default backgoud "#f6f6f6",  other //01b5d1          
                //AppbarBackground = "#2464f0",
                DrawerBackground = "#ffffffff",
                DrawerText = "#424242ff",




                PrimaryLighten = "#F2F9FE",
                SuccessLighten = "#F0FCF5",
                ErrorLighten = "#FEF4F3",
                WarningLighten = "#FFF9F0",
                DarkLighten = "#F7F7F7",


            },

            LayoutProperties = DefaultLayoutProperties
        };


    }

}

