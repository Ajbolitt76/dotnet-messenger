{{- define "fqdn-image" -}}
{{- if and .Values.inf.registry .Values.inf.registry.suffix -}}
{{- printf "%s/%s/%s" .Values.inf.registry.server .Values.inf.registry.suffix .Values.image.repository -}}
{{- else if .Values.inf.registry -}}
{{- printf "%s/%s" .Values.inf.registry.server .Values.image.repository -}}
{{- else -}}
{{- .Values.image.repository -}}
{{- end -}}
{{- end -}}