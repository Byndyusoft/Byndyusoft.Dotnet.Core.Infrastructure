FROM centos:7
MAINTAINER play-backend

RUN mkdir /etc/curator
ADD action_file.yml /etc/curator
ADD config.yml /etc/curator

# Install python-pip
RUN curl "https://bootstrap.pypa.io/get-pip.py" -o "get-pip.py"
RUN python get-pip.py

# Install curator (https://www.elastic.co/guide/en/elasticsearch/client/curator/5.6/pip.html)
RUN pip install elasticsearch-curator

# download go-cron
RUN curl -L -o /usr/local/bin/go-cron-linux.gz https://github.com/odise/go-cron/releases/download/v0.0.7/go-cron-linux.gz
RUN gunzip /usr/local/bin/go-cron-linux.gz
RUN chmod u+x /usr/local/bin/go-cron-linux

ENV PATH=/usr/local/bin:$PATH
ENV SCHEDULE "* * * * * *"
ENV COMMAND "echo test go-cron"
EXPOSE 8080
CMD go-cron-linux -s "$SCHEDULE" -p 8080 -- /bin/bash -c "$COMMAND"
