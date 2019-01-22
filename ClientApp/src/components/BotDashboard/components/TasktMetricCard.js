import React from 'react';
import MetricCard from './MetricCard';

export default class TasktMetricCard extends React.Component {
  constructor(props) {
      super(props);

      this.state = {
      error: null,
      isLoaded: false,
      metricName: props.metricName,
      api: props.api,
      metric: null,

      
      };

    }

    componentDidMount() {
        this.GetAPIData();
    }

    GetAPIData() {
        setTimeout(
            function () {
                this.GetAPIData();
            }
                .bind(this),
            1000
        );

        console.log('running ' + this.state.api);
        fetch(this.state.api)
            .then(res => res.json())
            .then(
                (result) => {
                    this.setState({
                        isLoaded: true,
                        metric: result
                    });
                },
                // Note: it's important to handle errors here
                // instead of a catch() block so that we don't swallow
                // exceptions from actual bugs in components.
                (error) => {
                    this.setState({
                        isLoaded: true,
                        error
                    });
                }
            )
    }

  

  render() {
      const { error, isLoaded, metricName, metric, descr } = this.state;

    if (error) {
        return <MetricCard metricName='Error!' metric='An error occured...'></MetricCard>;
    } else if (!isLoaded) {
        return <MetricCard metricName='Loading...' metric=''></MetricCard>;
    } else {
      return (
          <MetricCard metricName={metricName} metric={metric}></MetricCard>
      );
    }
  }
}