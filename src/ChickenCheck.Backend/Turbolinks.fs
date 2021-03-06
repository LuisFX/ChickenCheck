module ChickenCheck.Backend.Turbolinks

open Microsoft.AspNetCore.Http
open Giraffe
open Microsoft.Extensions.Primitives
open ChickenCheck.Backend.Extensions


module TurbolinksHelpers =

    let isTurbolink (ctx: HttpContext) =
        ctx.Request.Headers.ContainsKey "Turbolinks-Referrer"

    let internal handleTurbolinks (ctx: HttpContext) =
        if isTurbolink ctx then ctx.Response.Headers.Add("Turbolinks-Location", StringValues (ctx.FullPath))

///HttpHandler enabling Turbolinks support for given pipelines
let turbolinks (nxt: HttpFunc) (ctx: HttpContext): HttpFuncResult =
    TurbolinksHelpers.handleTurbolinks ctx
    nxt ctx
