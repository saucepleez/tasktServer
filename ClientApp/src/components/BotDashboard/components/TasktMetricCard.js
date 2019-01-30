import React from 'react';
import MetricCard from './MetricCard';
import Loader from '../../Loader';
export default class TasktMetricCard extends React.Component {
  constructor(props) {
      super(props);

      this.state = {
      error: null,
      isLoaded: false,
      metricName: props.metricName,
      api: props.api,
      metric: null,
      timelineScope: props.timelineScope,
      startDate: props.startDate
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
        var apiCall = this.state.api + "?startDate=" + this.state.startDate;
        console.log('running ' + apiCall);
        fetch(apiCall)
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
        return  <MetricCard metricName='Loading' metric='loading'></MetricCard>
    } else {
      return (
          <MetricCard metricName={metricName} metric={metric}></MetricCard>
      );
    }
  }
}