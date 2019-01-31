import React from 'react';
import ReactTable from 'react-table';
import 'react-table/react-table.css';
import Loader from '../../Loader';
export default class AssignmentList extends React.Component {
    displayName = AssignmentList.name
    constructor(props) {
        super(props);

        this.state = {
            error: null,
            isLoaded: false,
            assignmentList: [],
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
                        assignmentList: result
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
        const { error, isLoaded, assignmentList, api } = this.state;
        const divStyle = {
            backgroundColor: '#fff'
        };

        const columns = [    
            {
                Header: 'Assignment ID',
                accessor: 'assignmentID',
                Cell: row => <div><span title={row.value}>{row.value}</span></div>
            },
            {
                Header: 'Assignment Name',
                accessor: 'assignmentName',
                Cell: row => <div><span title={row.value}>{row.value}</span></div>
            },
            {
                Header: 'Enabled',
                accessor: 'enabled',
                Cell: row => <div><span title={row.value}>{row.value}</span></div>
            },
            {
                Header: 'Published Script ID',
                accessor: 'publishedScriptID',
                Cell: row => <div><span title={row.value}>{row.value}</span></div>
            },
            {
                Header: 'Assigned Worker/Pool',
                accessor: 'assignedWorker',
                Cell: row => <div><span title={row.value}>{row.value}</span></div>
            },
            {
                Header: 'Frequency',
                accessor: 'frequency',
                Cell: row => <div><span title={row.value}>{row.value}</span></div>
            },
            {
                Header: 'Interval',
                accessor: 'interval',
                Cell: row => <div><span title={row.value}>{row.value}</span></div>
            },
            {
                Header: 'New Task Due',
                accessor: 'newTaskDue',
                Cell: row => <div><span title={row.value}>{row.value}</span></div>
            }]


        if (error) {
            return <div>Error</div>;
        } else if (!isLoaded) {
            return <Loader type='spin' color='white' width='50px' height='50px'></Loader>;
        } else {
            return (

                <div style={divStyle}>
                    <ReactTable
                        data={assignmentList}
                        columns={columns}
                        defaultPageSize={20}
                        className="-striped -highlight"
                    />
                </div>

            );
        }
    }
}