import {
  DeleteButton,
  EditButton,
  List,
  MarkdownField,
  ShowButton,
  TextField,
  useTable,
} from "@refinedev/antd";
import { BaseRecord, useMany } from "@refinedev/core";
import { Space, Table } from "antd";

export const ProductList = () => {
  const { tableProps } = useTable({
    resource: "products",
    pagination: { current: 1, pageSize: 10 },
    syncWithLocation: true,
    
  });

  // console.log(tableQueryResult)
  // console.log(tableProps.dataSource)
  // const productsIds =
  //   tableProps.dataSource?.map((item) => item?.id) ?? [];

  const { isLoading } = useMany({
    resource: "products",
    ids:
      tableProps?.dataSource
        ?.map((item) => item?.data?.id)
        .filter(Boolean) ?? [],    
    queryOptions: {
      enabled: !!tableProps?.dataSource,
    },
  });

  return (
    <List>
      <Table {...tableProps} rowKey="id">
        <Table.Column dataIndex="id" title={"ID"} />
        <Table.Column
          dataIndex={"name"}
          title={"Name"}
          render={(value: string) =>
            isLoading? (
              <>Loading...</>
            ) : (
              <TextField
                value={<TextField value={value} />}
              />
            )
          }
        />
        <Table.Column
          dataIndex="description"
          title={"Description"}
          render={(value) => {
            if (!value) return "-";
            return <MarkdownField value={value.slice(0, 80) + "..."} />;
          }}
        />
        <Table.Column
          title={"Actions"}
          dataIndex="actions"
          render={(_, record: BaseRecord) => (
            <Space>
              <EditButton hideText size="small" recordItemId={record.id} />
              <ShowButton hideText size="small" recordItemId={record.id} />
              <DeleteButton hideText size="small" recordItemId={record.id} />
            </Space>
          )}
        />
      </Table>
    </List>
  );
};
