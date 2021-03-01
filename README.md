![main](https://github.com/dropsonic/aspnetcore-monitoringdemo/actions/workflows/main.yml/badge.svg?branch=main)

# ASP.NET Core Monitoring Demo

## Overview

ASP.NET Core logging and monitoring demo, based the default ASP.NET Core Web Application template from .NET 5.0, using [Docker](https://www.docker.com/) and [docker-compose](https://docs.docker.com/compose/) to put it all together.
This solution is, de-facto, a boilerplate for the full-blown logging and monitoring solution.

It consists of two main parts:

* Logging (ELK stack):
  * [Serilog](https://github.com/serilog/serilog) plus various additional Serilog packages, using [Serilog.Sinks.Http](https://github.com/FantasticFiasco/serilog-sinks-http) to send the data to [Logstash](https://github.com/elastic/logstash) via durable HTTP with batching and rolling file buffers
  * [Logstash](https://github.com/elastic/logstash) to ingest and filter the logs
  * [Elasticsearch](https://github.com/elastic/elasticsearch) to index the logs and provide searching capabilities
  * [Kibana](https://github.com/elastic/kibana) to visualize the logs
  
* Monitoring:
  * [Prometheus](https://github.com/prometheus/prometheus), including [Prometheus Pushgateway](https://github.com/prometheus/pushgateway), to collect and process all metrics
  * [prometheus-net](https://github.com/prometheus-net/prometheus-net) and [prometheus-net.DotNetRuntime](https://github.com/djluck/prometheus-net.DotNetRuntime) to collect ASP.NET Core and .NET Runtime-related metrics from the main container and to send them to [Prometheus](https://github.com/prometheus/prometheus)
  * [Grafana](https://github.com/grafana/grafana) with various predefined dashboards to visualize the metrics
  * [cAdvisor](https://github.com/google/cadvisor) to provide an understanding of the resource usage and performance characteristics of all running containers
  * [Node Exporter](https://github.com/prometheus/node_exporter) to expose hardware and OS metrics
  * [Alertmanager](https://github.com/prometheus/alertmanager) as the alerting system (no real configuration in the demo, just a blackhole receiver)

Also, it includes [ASP.NET Core Health Checks](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-5.0) based on [AspNetCore.Diagnostics.HealthChecks](https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks) and related packages. It is used to provided health checks for some internal metrics like memory consumption and for liveness of all the services. Also, it exposes a user interface for health checks on the main container.

## How To Run
`docker-compose up` or open `/MonitoringDemo.sln` in Visual Studio and run it as usual.

## Docker Containers Structure
![image](https://user-images.githubusercontent.com/1544021/109529760-14bddd80-7ac7-11eb-93b2-1b741290400b.png)

## How It Looks Like

### Default Swagger Endpoint
![image](https://user-images.githubusercontent.com/1544021/109526636-d96ddf80-7ac3-11eb-982f-fbe2abd45dd1.png)

### Health Checks UI
![image](https://user-images.githubusercontent.com/1544021/109526733-f30f2700-7ac3-11eb-849f-8958dad64a53.png)

### Logging (Kibana)
![image](https://user-images.githubusercontent.com/1544021/109527307-8ba5a700-7ac4-11eb-8d56-233193e5e836.png)
![image](https://user-images.githubusercontent.com/1544021/109527154-6749ca80-7ac4-11eb-9b7a-a900dda82636.png)

### Monitoring (Grafana)

#### Dashboards List
![image](https://user-images.githubusercontent.com/1544021/109527396-a546ee80-7ac4-11eb-936e-e6a2c8a2d444.png)

#### Health Checks
![image](https://user-images.githubusercontent.com/1544021/109528944-41252a00-7ac6-11eb-8f83-383a4a07e271.png)

#### .NET Runtime Metrics
![image](https://user-images.githubusercontent.com/1544021/109528300-96147080-7ac5-11eb-9378-f10bc7a410db.png)
![image](https://user-images.githubusercontent.com/1544021/109528360-a4628c80-7ac5-11eb-9ab1-4759636d8573.png)

#### ASP.NET Core Controller Summary
![image](https://user-images.githubusercontent.com/1544021/109528495-c65c0f00-7ac5-11eb-9ec7-415b98c2af2b.png)

#### prometheus-net
![image](https://user-images.githubusercontent.com/1544021/109528581-e390dd80-7ac5-11eb-84dd-40187fe6b6a2.png)

#### Docker Host
![image](https://user-images.githubusercontent.com/1544021/109528640-f60b1700-7ac5-11eb-9fe3-f5119a98aa96.png)
![image](https://user-images.githubusercontent.com/1544021/109528716-07ecba00-7ac6-11eb-94fb-2fa1acafeade.png)

#### Docker Containers
![image](https://user-images.githubusercontent.com/1544021/109529085-687bf700-7ac6-11eb-9f55-18620c69faa1.png)

#### Monitor Services (self-monitoring)
![image](https://user-images.githubusercontent.com/1544021/109529174-82b5d500-7ac6-11eb-8c43-3fb91f47b53d.png)

