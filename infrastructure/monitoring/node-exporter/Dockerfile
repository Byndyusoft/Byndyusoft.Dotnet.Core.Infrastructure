FROM prom/node-exporter:v0.15.2

ENV NODE_ID=none

USER root

COPY conf /etc/node-exporter/

ENTRYPOINT  ["sh",  "/etc/node-exporter/docker-entrypoint.sh" ]
CMD [ "/bin/node_exporter" ]
