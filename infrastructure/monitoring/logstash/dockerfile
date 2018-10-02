FROM logstash:5
RUN logstash-plugin install  logstash-output-email logstash-filter-multiline

COPY conf/logstash.conf /etc/logstash/conf.d/logstash.conf

CMD ["-f", "/etc/logstash/conf.d/logstash.conf"]