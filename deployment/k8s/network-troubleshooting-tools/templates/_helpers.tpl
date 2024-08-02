{{/*
Expand the name of the chart.
*/}}
{{- define "network-troubleshooting-tools.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "network-troubleshooting-tools.fullname" -}}
{{- if .Values.fullnameOverride }}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- $name := default .Chart.Name .Values.nameOverride }}
{{- if contains $name .Release.Name }}
{{- .Release.Name | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}
{{- end }}

{{/*
Create chart name and version as used by the chart label.
*/}}
{{- define "network-troubleshooting-tools.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "network-troubleshooting-tools.labels" -}}
helm.sh/chart: {{ include "network-troubleshooting-tools.chart" . }}
{{ include "network-troubleshooting-tools.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels
*/}}
{{- define "network-troubleshooting-tools.selectorLabels" -}}
app.kubernetes.io/name: {{ include "network-troubleshooting-tools.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
app: {{ include "network-troubleshooting-tools.name" . }}
tgs.kind: network
{{- end }}

{{/*
Create the name of the service account to use
*/}}
{{- define "network-troubleshooting-tools.serviceAccountName" -}}
{{- if .Values.serviceAccount.create }}
{{- default (include "network-troubleshooting-tools.fullname" .) .Values.serviceAccount.name }}
{{- else }}
{{- default "default" .Values.serviceAccount.name }}
{{- end }}
{{- end }}
