module Ruto.Utils.Config

open System
open dotenv.net

DotEnv.Load ()

let port =
  Environment.GetEnvironmentVariable ("PORT")
  |> Int32.Parse
