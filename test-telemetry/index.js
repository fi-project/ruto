const process = require('process');
const opentelemetry = require('@opentelemetry/api');
const { Resource } = require('@opentelemetry/resources');
const { SemanticResourceAttributes } = require('@opentelemetry/semantic-conventions');
const { BasicTracerProvider, ConsoleSpanExporter, SimpleSpanProcessor } = require('@opentelemetry/sdk-trace-base');
const { CollectorTraceExporter } = require('@opentelemetry/exporter-collector');
const { BatchSpanProcessor } = require('@opentelemetry/tracing');

let testEndpoint = 'http://localhost:7771/v1/trace';

if (process.env.TEST_ENDOPINT?.length > 0) {
  testEndpoint = process.env.TEST_ENDOPINT;
}

// const config = {
//   serviceName: 'test-telemetry',
//   logLevel: 'all',
//   metricExporter: 'none',
//   spanEndpoint,
// };

const provider = new BasicTracerProvider({
  resource: new Resource({
    [SemanticResourceAttributes.SERVICE_NAME]: 'test-telemetry',
  }),
});

const exporter = new CollectorTraceExporter({
  url: testEndpoint,
  headers: {},
});

provider.addSpanProcessor(new BatchSpanProcessor(exporter, {
  // The maximum queue size. After the size is reached spans are dropped.
  maxQueueSize: 100,
  // The maximum batch size of every export. It must be smaller or equal to maxQueueSize.
  maxExportBatchSize: 10,
  // The interval between two consecutive exports
  scheduledDelayMillis: 500,
  // How long the export can run before it is cancelled
  exportTimeoutMillis: 30000,
}));
provider.register();

let count = 0;
const tracer = opentelemetry.trace.getTracer('interval');

setInterval(() => {
  count++;
  console.log('start telemetry count:', count);
  const span = tracer.startSpan('test-span');
  span.setAttribute('count', count);
  span.addEvent('interval tick');
  span.end();

  tracer.getActiveSpanProcessor().forceFlush();

  console.log('end telemetry count:', count);
  if (count === Number.MAX_SAFE_INTEGER) {
    count = 0;
  }
}, 1000);

const shutdown = () => {
  exporter.shutdown();
  process.exit(0);
};

process.on('exit', shutdown);
process.on('SIGINT', shutdown);
process.on('SIGTERM', shutdown);
