![main](https://github.com/dropsonic/aspnetcore-monitoringdemo/actions/workflows/main.yml/badge.svg?branch=main)

# ASP.NET Core Monitoring Demo

## Overview

ASP.NET Core logging and monitoring demo, based the default ASP.NET Core Web Application template from .NET 5.0, using [Docker](https://www.docker.com/) and [docker-compose](https://docs.docker.com/compose/) to put it all together.
This solution is, de-facto, a boilerplate for the full-blown logging and monitoring solution.

It consists of two main parts:

* Logging (ELK stack):
  * [Elasticsearch](https://github.com/elastic/elasticsearch)
  * [Logstash](https://github.com/elastic/logstash)
  * [Kibana](https://github.com/elastic/kibana)
  * [Serilog](https://github.com/serilog/serilog) plus various additional Serilog packages, using [Serilog.Sinks.Http](https://github.com/FantasticFiasco/serilog-sinks-http) to send the data to [Logstash](https://github.com/elastic/logstash) via durable HTTP with batching and rolling file buffers
* Monitoring:
  * [Prometheus](https://github.com/prometheus/prometheus), including [Prometheus Pushgateway](https://github.com/prometheus/pushgateway)
  * [prometheus-net](https://github.com/prometheus-net/prometheus-net) and [prometheus-net.DotNetRuntime](https://github.com/djluck/prometheus-net.DotNetRuntime)
  * [Grafana](https://github.com/grafana/grafana) with various predefined dashboards
  * [cAdvisor](https://github.com/google/cadvisor)
  * [Node Exporter](https://github.com/prometheus/node_exporter)
  * [Alertmanager](https://github.com/prometheus/alertmanager)

Also, it includes [ASP.NET Core Health Checks](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-5.0) based on [AspNetCore.Diagnostics.HealthChecks](https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks) and related packages both for the checks and the UI.

## How To Run
`docker-compose up` or open `/MonitoringDemo.sln` in Visual Studio and run it as usual.

