import React from 'react';
import ReactTable from 'react-table';
import 'react-table/react-table.css';
import Loader from '../../Loader';
export default class ScriptList extends React.Component {
    displayName = ScriptList.name
    constructor(props) {
        super(props);

        this.state = {
            error: null,
            isLoaded: false,
            scriptList: [],
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
                        scriptList: result
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
        const { error, isLoaded, scriptList, api } = this.state;
        const divStyle = {
            backgroundColor: '#fff'
        };

        const columns = [    
            {
                Header: 'Worker ID',
                accessor: 'workerID',
                Cell: row => <div><span title={row.value}>{row.value}</span></div>
            },
            {
                Header: 'Worker Name',
                accessor: 'workerName',
                Cell: row => <div><span title={row.value}>{row.value}</span></div>
            },
            {
                Header: 'Machine Name',
                accessor: 'machineName',
                Cell: row => <div><span title={row.value}>{row.value}</span></div>
            },
            {
                Header: 'Script Name',
                accessor: 'friendlyName',
                Cell: row => <div><span title={row.value}>{row.value}</span></div>
            },
            {
                Header: 'Published On',
                accessor: 'publishedOn',
                Cell: row => <div><span title={row.value}>{row.value}</span></div>
            },
            {
                Header: 'Script Type',
                accessor: 'scriptType',
                Cell: row => <div><span title={row.value}>{row.value}</span></div>
            },
            {
                Header: 'Script Data',
                accessor: 'scriptData',
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
                        data={scriptList}
                        columns={columns}
                        defaultPageSize={20}
                        className="-striped -highlight"
                    />
                </div>

            );
        }
    }
}