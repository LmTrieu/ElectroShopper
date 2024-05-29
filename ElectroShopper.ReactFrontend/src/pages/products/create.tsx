import { Create, useForm, useSelect } from "@refinedev/antd";
import MDEditor from "@uiw/react-md-editor";
import { Form, Input, Select, InputNumber } from "antd";

export const ProductCreate = () => {
  const { formProps, saveButtonProps } = useForm({
    action: "create",
    resource: "products",
    meta: {
      headers: {
        "Content-Type": "multipart/form-data",
      },
      hasBody: false
    } 
  });

  const { selectProps: categorySelectProps } = useSelect({
    resource: "categories",  
    optionLabel: "name"
  });

  // const { selectProps: brandSelectProps } = useSelect({
  //   resource: "brands",  
  //   optionLabel: "Name"
  // });

  return (
    <Create saveButtonProps={saveButtonProps}>
      <Form {...formProps} layout="vertical">
        <Form.Item
          label={"Product's Name"}
          name={["name"]}
          rules={[
            {
              required: true,
            },
          ]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          label={"Description"}
          name="description"
          rules={[
            {
              required: true,
            },
          ]}
        >
          <MDEditor data-color-mode="light" />
        </Form.Item>
        <Form.Item  
          label={"Category"}
          name={["categoryId"]}
          rules={[
            {
              required: true,
            },
          ]}
        >
          <Select {...categorySelectProps} />
        </Form.Item>
        <Form.Item
          label={"Price"}
          name={["price"]}
          rules={[
            {        
              type: "number",
              min: 1,     
              required: true,
            },
          ]}
        >
          <InputNumber 
            prefix="₫"
            size = "large"
            formatter={(value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')}
            parser={(value) => value?.replace(/\$\s?|(,*)/g, '') as unknown as number} />
        </Form.Item>
        <Form.Item
          label={"Number of product"}
          name={["numOfProduct"]}
          rules={[
            {        
              type: "number",
              min: 1,     
              required: true,
            },
          ]}
        >
          <InputNumber 
            size = "large"/>
        </Form.Item>
        {/* <Form.Item  
          label={"Category"}
          name={["category", "id"]}
          rules={[
            {
              required: true,
            },
          ]}
        >
          <Select {...categorySelectProps} />
        </Form.Item> */}
      </Form>
    </Create>
  );
};
