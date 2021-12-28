module Ruto.Messaging.Consumers.RawConsumer

open Confluent.Kafka
open FsKafka
open Ruto.Messaging.Constants

module Config = Ruto.Utils.Config
module Logger = Ruto.Utils.Logger

let log = Logger.log

let handler (messages: ConsumeResult<string, string> []) =
  async {
    for m in messages do
      Logger.Debug (sprintf "Raw Telemetry Consumer Received: %s" m.Message.Value)
  }

let Listen () =
  let cfg =
    KafkaConsumerConfig.Create (
      Config.KafkaClientId,
      Config.KafkaHost,
      [ RawTopic ],
      RawConsumerGroup,
      AutoOffsetReset.Earliest
    )

  Logger.Info "Start raw telemetry consumer..."

  let consumer =
    BatchedConsumer.Start (log, cfg, handler)

  KafkaMonitor(log)
    .Start (consumer.Inner, cfg.Inner.GroupId)
  |> ignore

  consumer
