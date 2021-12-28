module Ruto.Utils.Config

open System
open dotenv.net

DotEnv.Load ()

let Debug =
  Environment.GetEnvironmentVariable("DEBUG").Length > 0

let Port =
  Environment.GetEnvironmentVariable ("PORT")
  |> Int32.Parse

let KafkaHost =
  Environment.GetEnvironmentVariable ("KAFKA_HOST")

let KafkaClientId =
  Environment.GetEnvironmentVariable ("KAFKA_CLIENT_ID")
