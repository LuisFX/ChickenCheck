module ChickenCheck.Backend.Bundle

    // Autogenerated from build.fsx, do not change!

    open Feliz.ViewEngine
    let bundle =
        [
            Html.script [ prop.src "app.js" ]
            Html.script [ prop.src "runtime.js" ]
            Html.script [ prop.src "style.js" ]
            Html.script [ prop.src "vendor.core-js.js" ]
            Html.script [ prop.src "vendor.css-loader.js" ]
            Html.script [ prop.src "vendor.fortawesome.js" ]
            Html.script [ prop.src "vendor.style-loader.js" ]
            Html.script [ prop.src "vendor.webpack.js" ]
        ]
