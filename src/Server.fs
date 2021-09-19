module Ruto.Server

open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.RequestErrors
open Ruto.Handlers

module Config = Utils.Config

let routes =
  choose [ GET >=> choose [ path "/ping" >=> OK "pong" ]
           POST
           >=> choose [ path "/v1/trace" >=> TraceHandler.trace ]
           request
             (fun r ->
               printfn "Not found: %s" r.path |> ignore
               NOT_FOUND "Resource not found.") ]

let start () =
  let serverConfig =
    { defaultConfig with
        bindings = [ HttpBinding.createSimple HTTP "127.0.0.1" Config.port ] }

  startWebServer serverConfig routes
