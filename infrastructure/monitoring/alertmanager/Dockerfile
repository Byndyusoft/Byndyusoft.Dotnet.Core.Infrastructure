FROM prom/alertmanager:v0.14.0

COPY conf /etc/alertmanager/

ENTRYPOINT  [ "sh", "/etc/alertmanager/docker-entrypoint.sh" ]
CMD        [ "--config.file=/etc/alertmanager/alertmanager.yml", \
             "--storage.path=/alertmanager" ]
