import React from 'react';
import ReactTable from 'react-table';
import 'react-table/react-table.css'

export default class TaskList extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            error: null,
            isLoaded: false,
            taskList: [],
            api: props.api,
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
                    console.log(result);
                    this.setState({
                        isLoaded: true,
                        taskList: result
                    });
                },
                // Note: it's important to handle errors here
                // instead of a catch() block so that we don't swallow
                // exceptions from actual bugs in components.
                (error) => {
                    console.log(error);
                    this.setState({
                        isLoaded: true,
                        error

                    });
                }
        )

    }

    render() {
        const { error, isLoaded, taskList, api } = this.state;
        const divStyle = {
            backgroundColor: '#fff'
         };

        const columns = [
            {
                Header: 'Task ID',
                accessor: 'taskID',
                Cell: row => <div><span title={row.value}>{row.value}</span></div>
           },
            {
                Header: 'Worker ID',
                accessor: 'workerID',
                Cell: row => <div><span title={row.value}>{row.value}</span></div>
            },
            {
                Header: 'Machine Name',
                accessor: 'machineName',
                Cell: row => <div><span title={row.value}>{row.value}</span></div>
            },
            {
                Header: 'User Name',
                accessor: 'userName',
                Cell: row => <div><span title={row.value}>{row.value}</span></div>
            },
            {
                Header: 'Task Name',
                accessor: 'taskName',
                Cell: row => <div><span title={row.value}>{row.value}</span></div>
            },
            {
                Header: 'Task Started',
                accessor: 'taskStarted',
                Cell: row => <div><span title={row.value}>{row.value}</span></div>
            },
            {
                Header: 'Task Finished',
                accessor: 'taskFinished',
                Cell: row => <div><span title={row.value}>{row.value}</span></div>
            },
            {
                Header: 'Status',
                accessor: 'status'
            }]


        if (error) {
            return <div>Error</div>;
        } else if (!isLoaded) {
            return <div>Loading</div>;
        } else {
            return (

                <div style={divStyle}>
                    <ReactTable
                        data={taskList}
                        columns={columns}
                        defaultPageSize={5}
                        className="-striped -highlight"               
                    />                   
                </div>

            );
        }
    }
}