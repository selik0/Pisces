# Pisces
自己写的玩的Unity框架   
1. UI界面自动创建赋值  
   1.1 命名规则, 支持同时获取多个组件。  
       示例：[variable_component1]或[variable_component1_component2]。  
       解释：使用[]包围确定该节点是否需要自动生成属性, variable表示变量名称， component1表示组件，可以有多个通过下划线_分隔。  
   1.2 Prefab需要放置在"Assets/PiscesBundles"文件夹下并且需要自动生成的Prefab的Root节点需要挂载PViewMarker脚本来进行识别  
   1.3 通过菜单栏的Pisces/UIPanel/自动生成功能来生成对应的界面脚本  
   1.4 通过菜单栏的Pisces/UIPanel/属性赋值功能来对生成的属性进行赋值  
