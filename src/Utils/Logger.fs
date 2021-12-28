module Ruto.Utils.Logger

open Serilog

let log =
  match Config.Debug with
  | true ->
    LoggerConfiguration()
      .MinimumLevel.Debug()
      .WriteTo.Console()
      .CreateLogger ()
  | false ->
    LoggerConfiguration()
      .MinimumLevel.Information()
      .WriteTo.Console()
      .CreateLogger ()

let Debug = log.Debug

let Info = log.Information

let Error = log.Error

let Warn = log.Warning
