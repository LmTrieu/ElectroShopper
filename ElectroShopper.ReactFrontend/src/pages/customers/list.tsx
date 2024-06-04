import {
  DeleteButton,
  EditButton,
  List,
  ShowButton,
  useTable,
} from "@refinedev/antd";
import { BaseRecord } from "@refinedev/core";
import { Space, Table } from "antd";

export const CustomerList = () => {
  const { tableProps } = useTable({
    resource: "customers",
    pagination: { current: 1, pageSize: 10 },
    syncWithLocation: true,
  });
  console.log(tableProps.dataSource)
  return (
    <List>
      <Table {...tableProps} rowKey="id">
        <Table.Column dataIndex="id" title={"ID"} />
        <Table.Column dataIndex="name" title={"Name"} />
        <Table.Column dataIndex="email" title={"Email"} />
      </Table>
    </List>
  );
};
