module Ruto.Handlers.TraceHandler

open Suave.Http
open Suave.Successful

let rec print (fields: list<string * option<string>>) =
  match fields with
  | head :: tail ->
    let () =
      (match head with
       | (key, Some (v)) -> printfn "key: %s; val: %s" key v |> ignore
       | (key, None) -> printfn "key: %s; val: null" key |> ignore)

    print tail
  | _ -> ()

let trace =
  request
    (fun req ->
      let reqData = req.form
      let () = print reqData
      OK "OK")
